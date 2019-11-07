using System;
using System.Collections.Generic;

namespace EventCalendar.Entities
{
    public class Event : IComparable<Event>
    {
        private string _title;
        private int _maxParticipants;
        private DateTime _dateTime;
        private Person _invitor;
        public List<Person> _participants;

        public string Title {
            get 
            {
                return _title;
            }
            set 
            {
                if (value != null)
                {
                    _title = value;
                }
                else
                {
                    throw new NullReferenceException(nameof(value));
                }
            }
        }

        public DateTime DateTime { get => _dateTime; set => _dateTime = value; }
        public Person Invitor { get => _invitor; set => _invitor = value; }
        public int MaxParticipants { get => _maxParticipants; set => _maxParticipants = value; }

        public Event(Person invitor, string title, DateTime dateTime, int participants = 0)
        {
            Invitor = invitor;
            Title = title;
            DateTime = dateTime;
            MaxParticipants = participants;
            _participants = new List<Person>();
        }

        public int CompareTo(Event other)
        {
            return this.DateTime.CompareTo(other.DateTime);
        }
    }
}
