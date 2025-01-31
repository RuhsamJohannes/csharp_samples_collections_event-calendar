﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using EventCalendar.Entities;
using static System.String;

namespace EventCalendar.Logic
{
    public class Controller
    {
        private readonly ICollection<Event> _events;
        public int EventsCount { get { return _events.Count; } }

        public Controller()
        {
            _events = new List<Event>();
        }

        /// <summary>
        /// Ein Event mit dem angegebenen Titel und dem Termin wird für den Einlader angelegt.
        /// Der Titel muss innerhalb der Veranstaltungen eindeutig sein und das Datum darf nicht
        /// in der Vergangenheit liegen.
        /// Mit dem optionalen Parameter maxParticipators kann eine Obergrenze für die Teilnehmer festgelegt
        /// werden.
        /// </summary>
        /// <param name="invitor"></param>
        /// <param name="title"></param>
        /// <param name="dateTime"></param>
        /// <param name="maxParticipators"></param>
        /// <returns>Wurde die Veranstaltung angelegt</returns>
        public bool CreateEvent(Person invitor, string title, DateTime dateTime, int maxParticipators = 0)
        {
            if (invitor == null || string.IsNullOrEmpty(title) || dateTime == null || dateTime < DateTime.Now || _events.Contains(GetEvent(title)))
            {
                return false;
            }
            else
            {
                _events.Add(new Event(invitor, title, dateTime, maxParticipators));
                return true;
            }
        }


        /// <summary>
        /// Liefert die Veranstaltung mit dem Titel
        /// </summary>
        /// <param name="title"></param>
        /// <returns>Event oder null, falls es keine Veranstaltung mit dem Titel gibt</returns>
        public Event GetEvent(string title)
        {
            foreach (Event item in _events)
            {
                if (item.Title.Equals(title))
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// Person registriert sich für Veranstaltung.
        /// Eine Person kann sich zu einer Veranstaltung nur einmal registrieren.
        /// </summary>
        /// <param name="person"></param>
        /// <param name="ev">Veranstaltung</param>
        /// <returns>War die Registrierung erfolgreich?</returns>
        public bool RegisterPersonForEvent(Person person, Event ev)
        {
            if (person == null || ev == null || ev._participants.Contains(person) || ev._participants.Count >= ev.MaxParticipants && ev.MaxParticipants != 0)
            {
                return false;
            }
            else 
            {
                ev._participants.Add(person);
                person._events.Add(ev);
                return true;
            }
        }

        /// <summary>
        /// Person meldet sich von Veranstaltung ab
        /// </summary>
        /// <param name="person"></param>
        /// <param name="ev">Veranstaltung</param>
        /// <returns>War die Abmeldung erfolgreich?</returns>
        public bool UnregisterPersonForEvent(Person person, Event ev)
        {
            if (person == null || ev == null || !ev._participants.Contains(person))
            {
                return false;
            }
            else
            {
                ev._participants.Remove(person);
                person._events.Remove(ev);
                return true;
            }
        }

        /// <summary>
        /// Liefert alle Teilnehmer an der Veranstaltung.
        /// Sortierung absteigend nach der Anzahl der Events der Personen.
        /// Bei gleicher Anzahl nach dem Namen der Person (aufsteigend).
        /// </summary>
        /// <param name="ev"></param>
        /// <returns>Liste der Teilnehmer oder null im Fehlerfall</returns>
        public IList<Person> GetParticipatorsForEvent(Event ev)
        {
            if (ev == null)
            {
                return null;
            }
            else
            {
                ev._participants.Sort();
                return ev._participants;
            }
        }

        /// <summary>
        /// Liefert alle Veranstaltungen der Person nach Datum (aufsteigend) sortiert.
        /// </summary>
        /// <param name="person"></param>
        /// <returns>Liste der Veranstaltungen oder null im Fehlerfall</returns>
        public List<Event> GetEventsForPerson(Person person)
        {
            if (person == null)
            {
                return null;
            }
            else
            {
                person._events.Sort();
                return person._events;
            }
        }

        /// <summary>
        /// Liefert die Anzahl der Veranstaltungen, für die die Person registriert ist.
        /// </summary>
        /// <param name="participator"></param>
        /// <returns>Anzahl oder 0 im Fehlerfall</returns>
        public int CountEventsForPerson(Person participator)
        {
            if (participator == null)
            {
                return 0;
            }
            else 
            { 
                return participator._events.Count;
            }
        }
    }
}
