using System;
using System.Collections.Generic;

class Program
{
    static List<Event> upcomingEvents = new List<Event>();
    static List<Event> completedEvents = new List<Event>();

    class Event
    {
        public string Title { get; set; }
        public string Date { get; set; }
        public string Priority { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ReminderMinutesBefore { get; set; }

        public Event(string title, string date, string priority, DateTime startTime, DateTime endTime, int reminderMinutesBefore)
        {
            Title = title;
            Date = date;
            Priority = priority;
            StartTime = startTime;
            EndTime = endTime;
            ReminderMinutesBefore = reminderMinutesBefore;
        }

        public override string ToString()
        {
            return $"{Title} (Date: {Date} | Priority: {Priority} | Start: {StartTime} | End: {EndTime} | Reminder: {ReminderMinutesBefore} minutes before)";
        }
    }

    static void DisplayEvents()
    {
        Console.WriteLine("\nUpcoming Events:");
        if (upcomingEvents.Count == 0)
            Console.WriteLine("No upcoming events.");
        for (int i = 0; i < upcomingEvents.Count; i++)
            Console.WriteLine($"{i + 1}. {upcomingEvents[i]}");

        Console.WriteLine("\nCompleted Events:");
        if (completedEvents.Count == 0)
            Console.WriteLine("No completed events.");
        for (int i = 0; i < completedEvents.Count; i++)
            Console.WriteLine($"{i + 1}. {completedEvents[i]}");

        Console.WriteLine();
    }

    static bool CheckForConflicts(DateTime newStartTime, DateTime newEndTime)
    {
        foreach (var ev in upcomingEvents)
        {
            
            if ((newStartTime < ev.EndTime && newEndTime > ev.StartTime))
            {
                return true;
            }
        }
        return false;
    }

    static void AddEvent()
    {
        Console.Write("Enter event title: ");
        string title = Console.ReadLine().Trim();

        Console.Write("Enter event date (YYYY-MM-DD): ");
        string date = Console.ReadLine().Trim();

        Console.Write("Enter priority (low, medium, high): ");
        string priority = Console.ReadLine().Trim().ToLower();

        Console.Write("Enter start time (HH:mm): ");
        DateTime startTime = DateTime.ParseExact(date + " " + Console.ReadLine().Trim(), "yyyy-MM-dd HH:mm", null);

        Console.Write("Enter end time (HH:mm): ");
        DateTime endTime = DateTime.ParseExact(date + " " + Console.ReadLine().Trim(), "yyyy-MM-dd HH:mm", null);

        
        if (CheckForConflicts(startTime, endTime))
        {
            Console.WriteLine("Error: This event conflicts with an existing event.");
            return;
        }

        Console.Write("Enter reminder time in minutes before event: ");
        int reminderMinutesBefore = int.Parse(Console.ReadLine().Trim());

        if (string.IsNullOrEmpty(title))
        {
            Console.WriteLine("Event title cannot be empty.");
            return;
        }

        if (priority != "low" && priority != "medium" && priority != "high")
            priority = "low";

        upcomingEvents.Add(new Event(title, date, priority, startTime, endTime, reminderMinutesBefore));
        Console.WriteLine("Event added successfully.");
    }

    static void EditEvent()
    {
        DisplayEvents();
        Console.Write("Edit from (upcoming/completed): ");
        string eventType = Console.ReadLine().Trim().ToLower();

        List<Event> eventList = eventType == "upcoming" ? upcomingEvents : eventType == "completed" ? completedEvents : null;
        if (eventList == null)
        {
            Console.WriteLine("Invalid event type.");
            return;
        }

        Console.Write("Enter event number to edit: ");
        if (!int.TryParse(Console.ReadLine(), out int eventIndex) || eventIndex <= 0 || eventIndex > eventList.Count)
        {
            Console.WriteLine("Invalid event number.");
            return;
        }

        Event ev = eventList[eventIndex - 1];
        Console.WriteLine($"Editing event: {ev}");

        Console.Write("Enter new event title: ");
        string title = Console.ReadLine().Trim();
        if (!string.IsNullOrEmpty(title))
            ev.Title = title;

        Console.Write("Enter new event date (YYYY-MM-DD): ");
        string date = Console.ReadLine().Trim();
        ev.Date = string.IsNullOrEmpty(date) ? ev.Date : date;

        Console.Write("Enter new priority (low, medium, high): ");
        string priority = Console.ReadLine().Trim().ToLower();
        if (priority == "low" || priority == "medium" || priority == "high")
            ev.Priority = priority;

        Console.Write("Enter new start time (HH:mm): ");
        DateTime startTime = DateTime.ParseExact(ev.Date + " " + Console.ReadLine().Trim(), "yyyy-MM-dd HH:mm", null);
        Console.Write("Enter new end time (HH:mm): ");
        DateTime endTime = DateTime.ParseExact(ev.Date + " " + Console.ReadLine().Trim(), "yyyy-MM-dd HH:mm", null);

        
        if (CheckForConflicts(startTime, endTime))
        {
            Console.WriteLine("Error: This event conflicts with an existing event.");
            return;
        }

        ev.StartTime = startTime;
        ev.EndTime = endTime;

        Console.Write("Enter new reminder time in minutes before event: ");
        int reminderMinutesBefore = int.Parse(Console.ReadLine().Trim());
        ev.ReminderMinutesBefore = reminderMinutesBefore;

        Console.WriteLine("Event updated successfully.");
    }

    static void DeleteEvent()
    {
        DisplayEvents();
        Console.Write("Delete from (upcoming/completed): ");
        string eventType = Console.ReadLine().Trim().ToLower();

        List<Event> eventList = eventType == "upcoming" ? upcomingEvents : eventType == "completed" ? completedEvents : null;
        if (eventList == null)
        {
            Console.WriteLine("Invalid event type.");
            return;
        }

        Console.Write("Enter event number to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int eventIndex) || eventIndex <= 0 || eventIndex > eventList.Count)
        {
            Console.WriteLine("Invalid event number.");
            return;
        }

        eventList.RemoveAt(eventIndex - 1);
        Console.WriteLine("Event deleted successfully.");
    }

    static void MarkEventAsCompleted()
    {
        DisplayEvents();
        Console.Write("Enter upcoming event number to mark as completed: ");
        if (!int.TryParse(Console.ReadLine(), out int eventIndex) || eventIndex <= 0 || eventIndex > upcomingEvents.Count)
        {
            Console.WriteLine("Invalid event number.");
            return;
        }

        Event ev = upcomingEvents[eventIndex - 1];
        upcomingEvents.RemoveAt(eventIndex - 1);
        completedEvents.Add(ev);
        Console.WriteLine("Event marked as completed.");
    }

    static void SearchEvents()
    {
        Console.Write("Enter search keyword: ");
        string query = Console.ReadLine().Trim().ToLower();

        Console.WriteLine("\nSearch Results:");
        foreach (var ev in upcomingEvents)
            if (ev.Title.ToLower().Contains(query))
                Console.WriteLine($"Upcoming: {ev}");

        foreach (var ev in completedEvents)
            if (ev.Title.ToLower().Contains(query))
                Console.WriteLine($"Completed: {ev}");

        Console.WriteLine();
    }

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("\nEvent Scheduler");
            Console.WriteLine("1. Display Events");
            Console.WriteLine("2. Add Event");
            Console.WriteLine("3. Edit Event");
            Console.WriteLine("4. Delete Event");
            Console.WriteLine("5. Mark Event as Completed");
            Console.WriteLine("6. Search Events");
            Console.WriteLine("7. Exit");

            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine().Trim();

            switch (choice)
            {
                case "1":
                    DisplayEvents();
                    break;
                case "2":
                    AddEvent();
                    break;
                case "3":
                    EditEvent();
                    break;
                case "4":
                    DeleteEvent();
                    break;
                case "5":
                    MarkEventAsCompleted();
                    break;
                case "6":
                    SearchEvents();
                    break;
                case "7":
                    Console.WriteLine("Exiting Event Scheduler. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}
