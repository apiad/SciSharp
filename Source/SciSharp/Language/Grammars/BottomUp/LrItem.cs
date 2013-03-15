using System;
using System.Collections.Generic;
using System.Text;


namespace SciSharp.Language.Grammars.BottomUp
{
    public class LrItem<T> : IEquatable<LrItem<T>>
        where T : Node, new()
    {
        public LrItem(Def<T> symbol, ProductionRule<T> production, HashSet<Token<T>> follow)
        {
            Item = new SlrItem<T> {Symbol = symbol, Body = production};
            Follow = follow;
        }

        public LrItem(SlrItem<T> item, HashSet<Token<T>> follow)
        {
            Item = item;
            Follow = follow;
        }

        public SlrItem<T> Item { get; set; }
        public HashSet<Token<T>> Follow { get; set; }

        public int Pointer
        {
            get { return Item.Pointer; }
            set { Item.Pointer = value; }
        }

        public Def<T> Symbol
        {
            get { return Item.Symbol; }
            set { Item.Symbol = value; }
        }

        public ProductionRule<T> Body
        {
            get { return Item.Body; }
            set { Item.Body = value; }
        }

        public Def<T> NextSymbol
        {
            get
            {
                if (Pointer < Body.Defs.Count)
                {
                    return Body.Defs[Pointer];
                }
                return null;
            }
        }

        public int Count
        {
            get { return Body.Count; }
        }

        #region IEquatable<LrItem<T>> Members

        public bool Equals(LrItem<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Item, Item);
        }

        #endregion

        public LrItem<T> Next()
        {
            return new LrItem<T>(Item.Next(), new HashSet<Token<T>>(Follow));
        }

        public bool TotalEquals(LrItem<T> other)
        {
            if (!Equals(other))
                return false;

            if (Follow.Count != other.Follow.Count)
                return false;

            foreach (var token in Follow)
            {
                if (!other.Follow.Contains(token))
                    return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (LrItem<T>)) return false;
            return Equals((LrItem<T>) obj);
        }

        public override int GetHashCode()
        {
            return (Item != null ? Item.GetHashCode() : 0);
        }

        public static bool operator ==(LrItem<T> left, LrItem<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LrItem<T> left, LrItem<T> right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Symbol);
            sb.Append(" -> ");

            for (int i = 0; i < Pointer; i++)
                sb.Append(Body.Defs[i] + " ");

            sb.Append(".");

            for (int i = Pointer; i < Body.Count; i++)
                sb.Append(Body.Defs[i] + " ");

            sb.Append(" | ");

            foreach (var token in Follow)
                sb.Append(token + ", ");

            return sb.ToString();
        }
    }
}
