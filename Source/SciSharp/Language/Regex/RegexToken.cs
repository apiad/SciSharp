using System;


namespace SciSharp.Language.Regex
{
    public struct RegexToken : IEquatable<RegexToken>
    {
        public RegexToken(RegexTokenType type, char value = '\0')
            : this()
        {
            Type = type;
            Value = value;
        }

        public RegexTokenType Type { get; private set; }
        public char Value { get; private set; }

        #region IEquatable<RegexToken> Members

        public bool Equals(RegexToken other)
        {
            return Equals(other.Type, Type) && other.Value == Value;
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (RegexToken)) return false;
            return Equals((RegexToken) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Type.GetHashCode()*397) ^ Value.GetHashCode();
            }
        }

        public static bool operator ==(RegexToken left, RegexToken right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RegexToken left, RegexToken right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return string.Format("Type: {0}, Value: {1}", Type, Value);
        }

        public static implicit operator RegexToken(RegexTokenType type)
        {
            return new RegexToken(type);
        }
    }
}
