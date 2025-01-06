from datetime import datetime, timedelta

events = {
    "upcoming": [],
    "completed": []
}

def display_events():
    print("\nUpcoming Events:")
    for i, event in enumerate(events["upcoming"], 1):
        print(f"{i}. {event['title']} (Start: {event['start_time']} | End: {event['end_time']} | Priority: {event['priority']} | Reminder: {event['reminder']})")

    print("\nCompleted Events:")
    for i, event in enumerate(events["completed"], 1):
        print(f"{i}. {event['title']} (Start: {event['start_time']} | End: {event['end_time']} | Priority: {event['priority']} | Reminder: {event['reminder']})")
    print("\n")

def is_conflicting(new_event_start, new_event_end):
   
    for event in events["upcoming"]:
        existing_start = datetime.strptime(event["start_time"], "%Y-%m-%d %H:%M")
        existing_end = datetime.strptime(event["end_time"], "%Y-%m-%d %H:%M")
        if not (new_event_end <= existing_start or new_event_start >= existing_end):
            return True
    return False

def add_event():
    title = input("Enter event title: ").strip()
    start_datetime = input("Enter event start time (YYYY-MM-DD HH:MM): ").strip()
    end_datetime = input("Enter event end time (YYYY-MM-DD HH:MM): ").strip()
    priority = input("Enter priority (low, medium, high): ").strip().lower()
    reminder_minutes = input("Enter reminder time in minutes before event (optional, default 15): ").strip()

    try:
        start_time = datetime.strptime(start_datetime, "%Y-%m-%d %H:%M")
        end_time = datetime.strptime(end_datetime, "%Y-%m-%d %H:%M")
    except ValueError:
        print("Invalid date and time format. Please use YYYY-MM-DD HH:MM.")
        return

    if end_time <= start_time:
        print("End time must be later than start time.")
        return

    
    if is_conflicting(start_time, end_time):
        print("Conflict detected! The event overlaps with an existing event.")
        return

    reminder = reminder_minutes if reminder_minutes else "15"
    event = {
        "title": title,
        "start_time": start_time.strftime("%Y-%m-%d %H:%M"),
        "end_time": end_time.strftime("%Y-%m-%d %H:%M"),
        "priority": priority if priority in ["low", "medium", "high"] else "low",
        "reminder": reminder
    }
    events["upcoming"].append(event)
    print("Event added successfully.")

def edit_event():
    display_events()
    event_type = input("Edit from (upcoming/completed): ").strip().lower()
    if event_type not in events:
        print("Invalid event type.")
        return

    event_index = int(input("Enter event number to edit: ")) - 1
    if event_index < 0 or event_index >= len(events[event_type]):
        print("Invalid event number.")
        return

    event = events[event_type][event_index]
    print(f"Editing event: {event['title']} (Start: {event['start_time']} | End: {event['end_time']} | Priority: {event['priority']} | Reminder: {event['reminder']})")

    event["title"] = input("Enter new event title: ").strip() or event["title"]
    start_datetime = input("Enter new event start time (YYYY-MM-DD HH:MM): ").strip()
    end_datetime = input("Enter new event end time (YYYY-MM-DD HH:MM): ").strip()

    try:
        start_time = datetime.strptime(start_datetime, "%Y-%m-%d %H:%M")
        end_time = datetime.strptime(end_datetime, "%Y-%m-%d %H:%M")
    except ValueError:
        print("Invalid date and time format. Please use YYYY-MM-DD HH:MM.")
        return

    if end_time <= start_time:
        print("End time must be later than start time.")
        return

    
    if is_conflicting(start_time, end_time):
        print("Conflict detected! The event overlaps with an existing event.")
        return

    reminder_minutes = input("Enter reminder time in minutes before event (optional, default 15): ").strip()
    event["reminder"] = reminder_minutes if reminder_minutes else "15"
    event["start_time"] = start_time.strftime("%Y-%m-%d %H:%M")
    event["end_time"] = end_time.strftime("%Y-%m-%d %H:%M")
    print("Event updated successfully.")

def delete_event():
    display_events()
    event_type = input("Delete from (upcoming/completed): ").strip().lower()
    if event_type not in events:
        print("Invalid event type.")
        return

    event_index = int(input("Enter event number to delete: ")) - 1
    if event_index < 0 or event_index >= len(events[event_type]):
        print("Invalid event number.")
        return

    events[event_type].pop(event_index)
    print("Event deleted successfully.")

def complete_event():
    display_events()
    event_index = int(input("Enter upcoming event number to mark as completed: ")) - 1
    if event_index < 0 or event_index >= len(events["upcoming"]):
        print("Invalid event number.")
        return

    event = events["upcoming"].pop(event_index)
    events["completed"].append(event)
    print("Event marked as completed.")

def search_events():
    query = input("Enter search keyword: ").strip().lower()
    print("\nSearch Results:")
    for event in events["upcoming"] + events["completed"]:
        if query in event["title"].lower():
            status = "Upcoming" if event in events["upcoming"] else "Completed"
            print(f"- {event['title']} (Start: {event['start_time']} | End: {event['end_time']} | Priority: {event['priority']} | Status: {status} | Reminder: {event['reminder']} minutes before)")
    print("\n")


while True:
    print("\nEvent Scheduler")
    print("1. Display Events")
    print("2. Add Event")
    print("3. Edit Event")
    print("4. Delete Event")
    print("5. Mark Event as Completed")
    print("6. Search Events")
    print("7. Exit")

    choice = input("Enter your choice: ").strip()

    if choice == "1":
        display_events()
    elif choice == "2":
        add_event()
    elif choice == "3":
        edit_event()
    elif choice == "4":
        delete_event()
    elif choice == "5":
        complete_event()
    elif choice == "6":
        search_events()
    elif choice == "7":
        print("Exiting Event Scheduler. Goodbye!")
        break
    else:
        print("Invalid choice. Please try again.")
