﻿namespace DotNetCoreWebApi.Backend.Model
{
    public class Owner : IOwner
    {
        private int _points = 0;
        public string Name { get; set; }

        public int Points
        {
            get => _points;
            set
            {
                if (_points != value)
                {
                    _points = value;
                }
            }
        }

        public Owner(string name)
        {
            Name = name;
        }

        public override bool Equals(object obj)
        {
            var other = (Owner)obj;
            return Points == other.Points && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
