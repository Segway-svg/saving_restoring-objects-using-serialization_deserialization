using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geolocation.Common.Extensions;
using Microsoft.Win32.SafeHandles;

namespace ConsoleApp1
{
    internal class Trash
    {
        public interface IVisitor
        {
            void Visit(Zoo zoo);
            void Visit(Cinema cinema);
            void Visit(Circus circus);
        }

        public interface IPlace
        {
           void Accept(IVisitor visitor);  
        }

        public class Zoo : IPlace
        {
            public void Accept(IVisitor visitor) => visitor.Visit(this);
        }

        public class Cinema : IPlace
        {
            public void Accept(IVisitor visitor) => visitor.Visit(this);
        }

        public class Circus : IPlace
        {
            public void Accept(IVisitor visitor) => visitor.Visit(this);
        }

        public class Holiday : IVisitor
        {
            public void Visit(Zoo zoo)
            {
                Console.WriteLine(1);
            }

            public void Visit(Cinema cinema)
            {
                Console.WriteLine(2);
            }

            public void Visit(Circus circus)
            {
                Console.WriteLine(3);
            }
        }
    }
}
