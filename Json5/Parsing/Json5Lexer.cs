using System;
using System.Globalization;
using System.IO;
using System.Numerics;

namespace Json5.Parsing
{
    /// <summary>
    /// Converts a stream of text into a stream of JSON5 tokens.
    /// </summary>
    class Json5Lexer
    {
        private readonly Json5TextReader reader;

        /// <summary>
        /// Constructs a new <see cref="Json5Lexer"/> using a <see cref="string"/>.
        /// </summary>
        /// <param name="source">The JSON5 text.</param>
        public Json5Lexer(string source) : this(new StringReader(source)) { }

        /// <summary>
        /// Constructs a new <see cref="Json5Lexer"/> using a <see cref="TextReader"/>.
        /// </summary>
        /// <param name="reader">The reader that reads JSON5 text.</param>
        public Json5Lexer(TextReader reader)
        {
            this.reader = new Json5TextReader(reader);
        }

        /// <summary>
        /// Gets the next token in the text.
        /// </summary>
        /// <returns>The next token in the text.</returns>
        public Json5Token Read()
        {
            var state = State.Default;
            var inputBuffer = "";
            var valueBuffer = "";
            double sign = 1;
            var doubleQuote = false;
            int? line = null;
            int? column = null;
            char c;

        start:
            var r = this.reader.Peek();

            switch (state)
            {
                case State.Default:
                    switch (r)
                    {
                        case '\t':
                        case '\v':
                        case '\f':
                        case ' ':
                        case 0x00A0:
                        case 0xFEFF:
                        case '\n':
                        case '\r':
                        case 0x2028:
                        case 0x2029:
                            // Skip whitespace.
                            // More whitespace is checked for at the end
                            // of this switch statement.
                            this.reader.Read();
                            goto start;

                        case '/':
                            state = State.Comment;
                            this.reader.Read();
                            goto start;

                        case '$':
                        case '_':
                            // More identifier characters are checked for
                            // at the end of this switch statement.
                            state = State.Identifier;
                            inputBuffer += c = (char)this.reader.Read();
                            valueBuffer += c;
                            goto start;

                        case '\\':
                            state = State.IdentifierStartEscapeSlash;
                            inputBuffer += (char)this.reader.Read();
                            goto start;

                        case '{':
                        case '}':
                        case '[':
                        case ']':
                        case ',':
                        case ':':
                            return Token(Json5TokenType.Punctuator, (char)this.reader.Read());

                        case '+':
                            state = State.Sign;
                            inputBuffer += (char)this.reader.Read();
                            goto start;

                        case '-':
                            state = State.Sign;
                            sign = -1;
                            inputBuffer += (char)this.reader.Read();
                            goto start;

                        case '0':
                            state = State.Zero;
                            inputBuffer += (char)this.reader.Read();
                            goto start;

                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                            state = State.DecimalInteger;
                            inputBuffer += (char)this.reader.Read();
                            goto start;

                        case '.':
                            state = State.DecimalPointLeading;
                            inputBuffer += (char)this.reader.Read();
                            goto start;

                        case '"':
                            state = State.String;
                            doubleQuote = true;
                            inputBuffer += (char)this.reader.Read();
                            line = this.reader.Line;
                            column = this.reader.Column;
                            goto start;

                        case '\'':
                            state = State.String;
                            inputBuffer += (char)this.reader.Read();
                            line = this.reader.Line;
                            column = this.reader.Column;
                            goto start;

                        case -1:
                            return Token(Json5TokenType.Eof);
                    }

                    if (char.GetUnicodeCategory((char)r) == UnicodeCategory.SpaceSeparator)
                    {
                        // Skip whitespace.
                        this.reader.Read();
                        goto start;
                    }

                    if (IsLetter((char)r))
                    {
                        state = State.Identifier;
                        inputBuffer += c = (char)this.reader.Read();
                        valueBuffer += c;
                        goto start;
                    }

                    throw UnexpectedCharacter((char)r);

                case State.Comment:
                    switch (r)
                    {
                        case '*':
                            state = State.MultiLineComment;
                            this.reader.Read();
                            goto start;

                        case '/':
                            state = State.SingleLineComment;
                            this.reader.Read();
                            goto start;

                        case -1:
                            throw UnexpectedEndOfInput();
                    }

                    throw UnexpectedCharacter((char)r);

                case State.MultiLineComment:
                    switch (r)
                    {
                        case '*':
                            state = State.MultiLineCommentAsterisk;
                            break;

                        case -1:
                            throw UnexpectedEndOfInput();
                    }

                    this.reader.Read();
                    goto start;

                case State.MultiLineCommentAsterisk:
                    if (r == -1)
                        throw UnexpectedEndOfInput();

                    state = r == '/' ? State.Default : State.MultiLineComment;
                    this.reader.Read();
                    goto start;

                case State.SingleLineComment:
                    switch (r)
                    {
                        case '\n':
                        case '\r':
                        case 0x2028:
                        case 0x2029:
                            state = State.Default;
                            break;

                        case -1:
                            return Token(Json5TokenType.Eof);
                    }

                    this.reader.Read();
                    goto start;

                case State.Identifier:
                    switch (r)
                    {
                        case '$':
                        case '_':
                            inputBuffer += c = (char)this.reader.Read();
                            valueBuffer += c;
                            goto start;

                        case '\\':
                            state = State.IdentifierEscapeSlash;
                            inputBuffer += (char)this.reader.Read();
                            goto start;
                    }

                    if (IsIdentifierChar((char)r))
                    {
                        inputBuffer += c = (char)this.reader.Read();
                        valueBuffer += c;
                        goto start;
                    }

                    return Token(Json5TokenType.Identifier, valueBuffer, inputBuffer);

                case State.IdentifierStartEscapeSlash:
                    if (r == -1)
                        throw UnexpectedEndOfInput();

                    if (r != 'u')
                        throw UnexpectedCharacter((char)r);

                    inputBuffer += (char)this.reader.Read();
                    var hexBuffer = "";
                    var count = 4;
                    while (count-- > 0)
                    {
                        if ((r = this.reader.Peek()) == -1)
                            throw UnexpectedEndOfInput();

                        if (!IsHexDigit(r))
                            throw UnexpectedCharacter((char)r);

                        inputBuffer += c = (char)this.reader.Read();
                        hexBuffer += c;
                    }

                    var u = (char)int.Parse(hexBuffer, NumberStyles.HexNumber);
                    if (u == '$' || u == '_' || IsLetter(u))
                    {
                        state = State.Identifier;
                        valueBuffer += u;
                        goto start;
                    }

                    throw InvalidUnicodeEscape(u);

                case State.IdentifierEscapeSlash:
                    if (r == -1)
                        throw UnexpectedEndOfInput();

                    if (r != 'u')
                        throw UnexpectedCharacter((char)r);

                    inputBuffer += (char)this.reader.Read();
                    hexBuffer = "";
                    count = 4;
                    while (count-- > 0)
                    {
                        if ((r = this.reader.Peek()) == -1)
                            throw UnexpectedEndOfInput();

                        if (!IsHexDigit(r))
                            throw UnexpectedCharacter((char)r);

                        inputBuffer += c = (char)this.reader.Read();
                        hexBuffer += c;
                    }

                    u = (char)int.Parse(hexBuffer, NumberStyles.HexNumber);
                    if (u == '$' || u == '_' || IsIdentifierChar(u))
                    {
                        state = State.Identifier;
                        valueBuffer += u;
                        goto start;
                    }

                    throw InvalidUnicodeEscape(u);

                case State.Sign:
                    switch (r)
                    {
                        case '0':
                            state = State.Zero;
                            inputBuffer += (char)this.reader.Read();
                            goto start;

                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                            state = State.DecimalInteger;
                            inputBuffer += (char)this.reader.Read();
                            goto start;

                        case '.':
                            state = State.DecimalPointLeading;
                            inputBuffer += (char)this.reader.Read();
                            goto start;

                        case 'I':
                            inputBuffer += (char)this.reader.Read();

                            foreach (var i in "nfinity")
                            {
                                if ((r = this.reader.Peek()) == -1)
                                    throw UnexpectedEndOfInput();

                                if (r != i)
                                    throw UnexpectedCharacter((char)r);

                                inputBuffer += (char)this.reader.Read();
                            }

                            return Token(Json5TokenType.Number, sign * double.PositiveInfinity, inputBuffer);

                        case 'N':
                            inputBuffer += (char)this.reader.Read();

                            if ((r = this.reader.Peek()) == -1)
                                throw UnexpectedEndOfInput();

                            if (r != 'a')
                                throw UnexpectedCharacter((char)r);

                            inputBuffer += (char)this.reader.Read();

                            if ((r = this.reader.Peek()) == -1)
                                throw UnexpectedEndOfInput();

                            if (r != 'N')
                                throw UnexpectedCharacter((char)r);

                            inputBuffer += (char)this.reader.Read();
                            return Token(Json5TokenType.Number, double.NaN, inputBuffer);

                        case -1:
                            throw UnexpectedEndOfInput();
                    }

                    throw UnexpectedCharacter((char)r);

                case State.Zero:
                    switch (r)
                    {
                        case '.':
                            state = State.DecimalPoint;
                            inputBuffer += (char)this.reader.Read();
                            goto start;

                        case 'e':
                        case 'E':
                            state = State.DecimalExponent;
                            inputBuffer += (char)this.reader.Read();
                            goto start;

                        case 'x':
                        case 'X':
                            state = State.Hexadecimal;
                            inputBuffer += (char)this.reader.Read();
                            valueBuffer = "";
                            goto start;

                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                            throw Error(new NotSupportedException("Octal numbers are not supported."));

                        // Node.js supports the following; JSON does not:
                        case '8':
                        case '9':
                            throw Error(new InvalidOperationException("Integer parts must not begin with 0."));
                    }

                    return Token(Json5TokenType.Number, 0D, inputBuffer);

                case State.DecimalInteger:
                    switch (r)
                    {
                        case '.':
                            state = State.DecimalPoint;
                            inputBuffer += (char)this.reader.Read();
                            goto start;

                        case 'e':
                        case 'E':
                            state = State.DecimalExponent;
                            inputBuffer += (char)this.reader.Read();
                            goto start;
                    }

                    if (IsDigit(r))
                    {
                        inputBuffer += (char)this.reader.Read();
                        goto start;
                    }

                    return Token(Json5TokenType.Number, double.Parse(inputBuffer, CultureInfo.InvariantCulture), inputBuffer);

                case State.DecimalPointLeading:
                    if (r == -1)
                        throw UnexpectedEndOfInput();

                    if (IsDigit(r))
                    {
                        state = State.DecimalFraction;
                        inputBuffer += (char)this.reader.Read();
                        goto start;
                    }

                    throw UnexpectedCharacter((char)r);

                case State.DecimalPoint:
                    switch (r)
                    {
                        case 'e':
                        case 'E':
                            state = State.DecimalExponent;
                            inputBuffer += (char)this.reader.Read();
                            goto start;
                    }

                    if (IsDigit(r))
                    {
                        state = State.DecimalFraction;
                        inputBuffer += (char)this.reader.Read();
                        goto start;
                    }

                    return Token(Json5TokenType.Number, double.Parse(inputBuffer, CultureInfo.InvariantCulture), inputBuffer);

                case State.DecimalFraction:
                    switch (r)
                    {
                        case 'e':
                        case 'E':
                            state = State.DecimalExponent;
                            inputBuffer += (char)this.reader.Read();
                            goto start;
                    }

                    if (IsDigit(r))
                    {
                        inputBuffer += (char)this.reader.Read();
                        goto start;
                    }

                    return Token(Json5TokenType.Number, double.Parse(inputBuffer, CultureInfo.InvariantCulture), inputBuffer);

                case State.DecimalExponent:
                    switch (r)
                    {
                        case '+':
                        case '-':
                            state = State.DecimalExponentSign;
                            inputBuffer += (char)this.reader.Read();
                            goto start;

                        case -1:
                            throw UnexpectedEndOfInput();
                    }

                    if (IsDigit(r))
                    {
                        state = State.DecimalExponentInteger;
                        inputBuffer += (char)this.reader.Read();
                        goto start;
                    }

                    throw UnexpectedCharacter((char)r);

                case State.DecimalExponentSign:
                    if (r == -1)
                        throw UnexpectedEndOfInput();

                    if (IsDigit(r))
                    {
                        state = State.DecimalExponentInteger;
                        inputBuffer += (char)this.reader.Read();
                        goto start;
                    }

                    throw UnexpectedCharacter((char)r);

                case State.DecimalExponentInteger:
                    if (IsDigit(r))
                    {
                        inputBuffer += (char)this.reader.Read();
                        goto start;
                    }

                    return Token(Json5TokenType.Number, double.Parse(inputBuffer, CultureInfo.InvariantCulture), inputBuffer);

                case State.Hexadecimal:
                    if (r == -1)
                        throw UnexpectedEndOfInput();

                    if (IsHexDigit(r))
                    {
                        state = State.HexadecimalInteger;
                        inputBuffer += c = (char)this.reader.Read();
                        valueBuffer += c;
                        goto start;
                    }

                    throw UnexpectedCharacter((char)r);

                case State.HexadecimalInteger:
                    if (IsHexDigit(r))
                    {
                        state = State.HexadecimalInteger;
                        inputBuffer += c = (char)this.reader.Read();
                        valueBuffer += c;
                        goto start;
                    }

                    // Parse this value with BigInteger because ulong can only parse numbers up to 0xFFFFFFFFFFFFFFFF.
                    // Add '0' to valueBuffer since otherwise it will be parsed as signed value (e.g. ff would be -1) 
                    return Token(Json5TokenType.Number, sign * (double)BigInteger.Parse("0" + valueBuffer, NumberStyles.HexNumber), inputBuffer);

                case State.String:
                    switch (r)
                    {
                        case '\\':
                            state = State.Escape;
                            inputBuffer += (char)this.reader.Read();
                            goto start;

                        case '"':
                            if (doubleQuote)
                            {
                                inputBuffer += (char)this.reader.Read();
                                return Token(Json5TokenType.String, valueBuffer, inputBuffer, line, column);
                            }

                            inputBuffer += c = (char)this.reader.Read();
                            valueBuffer += c;
                            goto start;

                        case '\'':
                            if (!doubleQuote)
                            {
                                inputBuffer += (char)this.reader.Read();
                                return Token(Json5TokenType.String, valueBuffer, inputBuffer, line, column);
                            }

                            inputBuffer += c = (char)this.reader.Read();
                            valueBuffer += c;
                            goto start;

                        case '\n':
                        case '\r':
                            //case 0x2028: // These are not supported in strings in ES5, but they are in JSON.
                            //case 0x2029: // For backward compatibility, we allow them but never stringify them unescaped.
                            throw UnexpectedCharacter((char)r);

                        case -1:
                            throw UnexpectedEndOfInput();
                    }

                    inputBuffer += c = (char)this.reader.Read();
                    valueBuffer += c;
                    goto start;

                case State.Escape:
                    switch (r)
                    {
                        case 'b':
                            state = State.String;
                            valueBuffer += '\b';
                            inputBuffer += (char)this.reader.Read();
                            goto start;

                        case 'f':
                            state = State.String;
                            valueBuffer += '\f';
                            inputBuffer += (char)this.reader.Read();
                            goto start;

                        case 'n':
                            state = State.String;
                            valueBuffer += '\n';
                            inputBuffer += (char)this.reader.Read();
                            goto start;

                        case 'r':
                            state = State.String;
                            valueBuffer += '\r';
                            inputBuffer += (char)this.reader.Read();
                            goto start;

                        case 't':
                            state = State.String;
                            valueBuffer += '\t';
                            inputBuffer += (char)this.reader.Read();
                            goto start;

                        case 'v':
                            state = State.String;
                            valueBuffer += '\v';
                            inputBuffer += (char)this.reader.Read();
                            goto start;

                        case '0': // lookahead
                            inputBuffer += (char)this.reader.Read();
                            if (IsDigit(this.reader.Peek()))
                                throw Error(new NotSupportedException("Octal escape sequences are not supported."));

                            state = State.String;
                            valueBuffer += '\0';
                            goto start;

                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                            throw Error(new NotSupportedException("Octal escape sequences are not supported."));

                        case '8':
                        case '9':
                            throw UnexpectedCharacter((char)r);

                        case 'x':
                            inputBuffer += (char)this.reader.Read();
                            hexBuffer = "";

                            if ((r = this.reader.Peek()) == -1)
                                throw UnexpectedEndOfInput();

                            if (!IsHexDigit(r))
                                throw UnexpectedCharacter((char)r);

                            inputBuffer += c = (char)this.reader.Read();
                            hexBuffer += c;

                            if ((r = this.reader.Peek()) == -1)
                                throw UnexpectedEndOfInput();

                            if (!IsHexDigit(r))
                                throw UnexpectedCharacter((char)r);

                            inputBuffer += c = (char)this.reader.Read();
                            hexBuffer += c;
                            valueBuffer += (char)int.Parse(hexBuffer, NumberStyles.HexNumber);
                            state = State.String;
                            goto start;

                        case 'u':
                            inputBuffer += (char)this.reader.Read();
                            hexBuffer = "";
                            count = 4;
                            while (count-- > 0)
                            {
                                if ((r = this.reader.Peek()) == -1)
                                    throw UnexpectedEndOfInput();

                                if (!IsHexDigit(r))
                                    throw UnexpectedCharacter((char)r);

                                inputBuffer += c = (char)this.reader.Read();
                                hexBuffer += c;
                            }

                            valueBuffer += (char)int.Parse(hexBuffer, NumberStyles.HexNumber);
                            state = State.String;
                            goto start;

                        case '\n':
                        case '\r':
                        case 0x2028:
                        case 0x2029:
                            state = State.String;
                            inputBuffer += (char)this.reader.Read();
                            if (r == '\r' && this.reader.Peek() == '\n')
                                inputBuffer += (char)this.reader.Read();

                            goto start;

                        case -1:
                            throw UnexpectedEndOfInput();
                    }

                    state = State.String;
                    inputBuffer += c = (char)this.reader.Read();
                    valueBuffer += c;
                    goto start;
            }

            // If this happens, it's a programming error.
            throw Error(new InvalidOperationException("Invalid lexer state"));
        }

        /// <summary>
        /// Determines whether a character is a decimal digit.
        /// </summary>
        /// <param name="c">The character to test.</param>
        /// <returns><c>true</c> if <paramref name="c"/> is a digit; otherwise, <c>false</c>.</returns>
        static bool IsDigit(int c)
        {
            return c switch
            {
                '0' or
                '1' or
                '2' or
                '3' or
                '4' or
                '5' or
                '6' or
                '7' or
                '8' or
                '9' => true,
                _ => false,
            };
        }

        /// <summary>
        /// Determines whether a character is a hexadecimal digit.
        /// </summary>
        /// <param name="c">The character to test.</param>
        /// <returns><c>true</c> if <paramref name="c"/> is a hexadecimal digit; otherwise, <c>false</c>.</returns>
        static bool IsHexDigit(int c)
        {
            return c switch
            {
                'a' or 'A' or
                'b' or 'B' or
                'c' or 'C' or
                'd' or 'D' or
                'e' or 'E' or
                'f' or 'F' => true,
                _ => IsDigit(c),
            };
        }

        /// <summary>
        /// Determines whether a character is a Unicode letter character.
        /// </summary>
        /// <param name="c">The character to test.</param>
        /// <returns><c>true</c> if <paramref name="c"/> is a Unicode letter character; otherwise, <c>false</c>.</returns>
        static bool IsLetter(char c)
        {
            return char.IsLetter(c) || char.GetUnicodeCategory(c) == UnicodeCategory.LetterNumber;
        }

        /// <summary>
        /// Determines whether a character is an ECMAScript 5.1 Unicode identifier character.
        /// </summary>
        /// <param name="c">The character to test.</param>
        /// <returns><c>true</c> if <paramref name="c"/> is an ECMAScript 5.1 Unicode identifier character; otherwise, <c>false</c>.</returns>
        static bool IsIdentifierChar(char c)
        {
            return char.GetUnicodeCategory(c) switch
            {
                UnicodeCategory.NonSpacingMark or
                UnicodeCategory.SpacingCombiningMark or
                UnicodeCategory.DecimalDigitNumber or
                UnicodeCategory.ConnectorPunctuation or
                UnicodeCategory.Format => true,
                _ => IsLetter(c),
            };
        }

        /// <summary>
        /// Creates a new <see cref="Json5Token"/>.
        /// </summary>
        /// <param name="type">The type of the token.</param>
        /// <param name="value">The value of the token.</param>
        /// <param name="input">The input string that generated the token.</param>
        /// <param name="line">The line number where the token was found.</param>
        /// <param name="column">The column number where the token was found.</param>
        /// <returns>A new <see cref="Json5Token"/>.</returns>
        Json5Token Token(Json5TokenType type, object value = null, string input = null, int? line = null, int? column = null)
        {
            if (value != null)
            {
                input ??= value.ToString();
                column ??= this.reader.Column - input.Length;
            }

            return new Json5Token { Type = type, Value = value, Input = input, Line = line ?? this.reader.Line, Column = column ?? this.reader.Column };
        }

        /// <summary>
        /// Returns a <see cref="Json5UnexpectedInputException"/> for an unexpected character.
        /// </summary>
        /// <param name="c">The unexpected character.</param>
        /// <returns>A <see cref="Json5UnexpectedInputException"/> for <paramref name="c"/>.</returns>
        Json5UnexpectedInputException UnexpectedCharacter(char c)
        {
            this.reader.Read();
            return new Json5UnexpectedInputException(c.ToString(), this.reader.Line, this.reader.Column);
        }

        /// <summary>
        /// Returns a <see cref="Json5UnexpectedEndOfInputException"/> for an unexpected end of input.
        /// </summary>
        /// <returns>A <see cref="Json5UnexpectedEndOfInputException"/>.</returns>
        Json5UnexpectedEndOfInputException UnexpectedEndOfInput()
        {
            return new Json5UnexpectedEndOfInputException(this.reader.Line, this.reader.Column);
        }

        /// <summary>
        /// Returns a <see cref="Json5ParsingException"/> for a general error.
        /// </summary>
        /// <param name="innerException">The inner exception that caused the error.</param>
        /// <returns>A <see cref="Json5ParsingException"/>.</returns>
        Json5ParsingException Error(Exception innerException)
        {
            this.reader.Read();
            return new Json5ParsingException(innerException, this.reader.Line, this.reader.Column);
        }

        /// <summary>
        /// Returns a <see cref="Json5ParsingException"/> for an invalid Unicode escape sequence.
        /// </summary>
        /// <param name="u">The invalid Unicode character.</param>
        /// <returns>A <see cref="Json5ParsingException"/>.</returns>
        Json5ParsingException InvalidUnicodeEscape(char u)
        {
            this.reader.Read();
            return new Json5ParsingException(string.Format("Invalid unicode escape sequence '\\u{0}' in object key.", ((int)u).ToString("x4")), this.reader.Line, this.reader.Column - 6);
        }

        /// <summary>
        /// Represents a state that a <see cref="Json5Lexer"/> is in.
        /// </summary>
        enum State
        {
            /// <summary>
            /// The default state.
            /// </summary>
            Default,

            /// <summary>
            /// The state when a comment is detected.
            /// </summary>
            Comment,

            /// <summary>
            /// The state when a multi-line comment is detected.
            /// </summary>
            MultiLineComment,

            /// <summary>
            /// The state when an asterisk is detected in a multi-line comment.
            /// </summary>
            MultiLineCommentAsterisk,

            /// <summary>
            /// The state when a single-line comment is detected.
            /// </summary>
            SingleLineComment,

            /// <summary>
            /// The state when an identifier is detected.
            /// </summary>
            Identifier,

            /// <summary>
            /// The state when an escape slash is detected at the start of an identifier.
            /// </summary>
            IdentifierStartEscapeSlash,

            /// <summary>
            /// The state when an escape slash is detected in an identifier.
            /// </summary>
            IdentifierEscapeSlash,

            /// <summary>
            /// The state when a sign is detected.
            /// </summary>
            Sign,

            /// <summary>
            /// The state when a zero is detected.
            /// </summary>
            Zero,

            /// <summary>
            /// The state when a decimal integer is detected.
            /// </summary>
            DecimalInteger,

            /// <summary>
            /// The state when a leading decimal point is detected.
            /// </summary>
            DecimalPointLeading,

            /// <summary>
            /// The state when a decimal point is detected.
            /// </summary>
            DecimalPoint,

            /// <summary>
            /// The state when a decimal fraction is detected.
            /// </summary>
            DecimalFraction,

            /// <summary>
            /// The state when a decimal exponent is detected.
            /// </summary>
            DecimalExponent,

            /// <summary>
            /// The state when a sign is detected in a decimal exponent.
            /// </summary>
            DecimalExponentSign,

            /// <summary>
            /// The state when an integer is detected in a decimal exponent.
            /// </summary>
            DecimalExponentInteger,

            /// <summary>
            /// The state when a hexadecimal number is detected.
            /// </summary>
            Hexadecimal,

            /// <summary>
            /// The state when a hexadecimal integer is detected.
            /// </summary>
            HexadecimalInteger,

            /// <summary>
            /// The state when a string is detected.
            /// </summary>
            String,

            /// <summary>
            /// The state when an escape character is detected in a string.
            /// </summary>
            Escape,
        }
    }
}
