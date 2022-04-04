using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.DataStructure
{
    public class Category : IComparable
    {
        string Text { get; }
        MessageType MessageType { get; }
        MessageTopic MessageTopic { get; }

        public Category(string text, MessageType messageType, MessageTopic messageTopic)
        {
            Text = text;
            MessageType = messageType;
            MessageTopic = messageTopic;
        }

        public bool Equals(Category category)
        {
            if (category is null)
                return false;
            else
                return Text == category.Text
                    && MessageType == category.MessageType
                    && MessageTopic == category.MessageTopic;
        }

        public int CompareTo(object o)
        {
            if (o is Category category)
            {
                var x1 = (Text is null) ? 0 : Text.CompareTo(category.Text);
                if (x1 != 0) return x1;

                var x2 = MessageType.CompareTo(category.MessageType);
                if(x2 != 0) return x2;

                var x3 = MessageTopic.CompareTo(category.MessageTopic);
                return x3;
            }

            else return 0;
        }

        public override string ToString()
        {
            return Text + '.' + MessageType + '.' + MessageTopic;
        }

        public static bool operator <=(Category x, Category y)
        {
            if (x is null || y is null) return true;
            else return (x.CompareTo(y) == -1 || x.CompareTo(y) == 0) ? true : false;
        }

        public static bool operator >=(Category x, Category y)
        {
            if (x is null || y is null) return true;
            else return (x.CompareTo(y) == 1 || x.CompareTo(y) == 0) ? true : false;
        }

        public static bool operator >(Category x, Category y)
        {
            if (x is null || y is null) return true;
            else return (x.CompareTo(y) == 1) ? true : false;
        }

        public static bool operator <(Category x, Category y)
        {
            if (x is null || y is null) return true;
            else return (x.CompareTo(y) == -1) ? true : false;
        }

        public static bool operator ==(Category x, Category y)
        {
            if (x is null || y is null) return true;
            else return (x.CompareTo(y) == 0) ? true : false;
        }

        public static bool operator !=(Category x, Category y)
        {
            if (x is null || y is null) return true;
            else return (x.CompareTo(y) != 0) ? true : false;
        }
    }
}