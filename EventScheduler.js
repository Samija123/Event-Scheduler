const readline = require("readline");

const events = {
  upcoming: [],
  completed: []
};


const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});


function displayEvents() {
  console.log("\n--- Upcoming Events ---");
  if (events.upcoming.length === 0) {
    console.log("No upcoming events.");
  } else {
    events.upcoming.forEach((event, index) => {
      console.log(
        `${index + 1}. ${event.title} (Start: ${event.startTime} | End: ${event.endTime} | Priority: ${event.priority} | Reminder: ${event.reminder} minutes before)`
      );
    });
  }

  console.log("\n--- Completed Events ---");
  if (events.completed.length === 0) {
    console.log("No completed events.");
  } else {
    events.completed.forEach((event, index) => {
      console.log(
        `${index + 1}. ${event.title} (Start: ${event.startTime} | End: ${event.endTime} | Priority: ${event.priority} | Reminder: ${event.reminder} minutes before)`
      );
    });
  }
  console.log("\n");
}

function isConflicting(newEventStartTime, newEventEndTime) {
  
  for (const event of events.upcoming) {
    const existingStartTime = new Date(event.startTime);
    const existingEndTime = new Date(event.endTime);
    
    if (
      (newEventStartTime < existingEndTime && newEventStartTime >= existingStartTime) || // New event starts during an existing event
      (newEventEndTime > existingStartTime && newEventEndTime <= existingEndTime) // New event ends during an existing event
    ) {
      return true;
    }
  }
  return false;
}

function addEvent() {
  rl.question("Enter event title: ", (title) => {
    if (!title.trim()) {
      console.log("Event title cannot be empty.");
      return mainMenu();
    }

    rl.question("Enter event start time (YYYY-MM-DD HH:MM): ", (startTimeInput) => {
      rl.question("Enter event end time (YYYY-MM-DD HH:MM): ", (endTimeInput) => {
        rl.question("Enter priority (low, medium, high): ", (priorityInput) => {
          const priority = ["low", "medium", "high"].includes(priorityInput.toLowerCase())
            ? priorityInput.toLowerCase()
            : "low";

          rl.question("Enter reminder time in minutes before event (optional, default 15): ", (reminderInput) => {
            const reminder = reminderInput.trim() ? parseInt(reminderInput) : 15;

            const startTime = new Date(startTimeInput);
            const endTime = new Date(endTimeInput);

            if (isNaN(startTime) || isNaN(endTime)) {
              console.log("Invalid date and time format. Please use YYYY-MM-DD HH:MM.");
              return mainMenu();
            }

            if (endTime <= startTime) {
              console.log("End time must be later than start time.");
              return mainMenu();
            }

            
            if (isConflicting(startTime, endTime)) {
              console.log("Conflict detected! The event overlaps with an existing event.");
              return mainMenu();
            }

            const event = {
              title,
              startTime: startTime.toISOString(),
              endTime: endTime.toISOString(),
              priority,
              reminder
            };
            events.upcoming.push(event);
            console.log("Event added successfully!");
            mainMenu();
          });
        });
      });
    });
  });
}

function editEvent() {
  displayEvents();
  rl.question("Enter the number of the event to edit: ", (index) => {
    const eventIndex = parseInt(index) - 1;
    if (eventIndex < 0 || eventIndex >= events.upcoming.length) {
      console.log("Invalid event number.");
      return mainMenu();
    }

    const event = events.upcoming[eventIndex];
    console.log(`Editing Event: ${event.title} (Start: ${event.startTime} | End: ${event.endTime} | Priority: ${event.priority} | Reminder: ${event.reminder})`);

    rl.question("Enter new title (or press Enter to keep current): ", (title) => {
      rl.question("Enter new start time (YYYY-MM-DD HH:MM, or press Enter to keep current): ", (startTimeInput) => {
        rl.question("Enter new end time (YYYY-MM-DD HH:MM, or press Enter to keep current): ", (endTimeInput) => {
          rl.question("Enter new priority (low, medium, high, or press Enter to keep current): ", (priorityInput) => {
            rl.question("Enter new reminder time in minutes before event (or press Enter to keep current): ", (reminderInput) => {

              if (title.trim()) event.title = title;
              if (startTimeInput.trim()) event.startTime = new Date(startTimeInput).toISOString();
              if (endTimeInput.trim()) event.endTime = new Date(endTimeInput).toISOString();
              if (["low", "medium", "high"].includes(priorityInput.toLowerCase())) event.priority = priorityInput.toLowerCase();
              if (reminderInput.trim()) event.reminder = parseInt(reminderInput);

              console.log("Event updated successfully!");
              mainMenu();
            });
          });
        });
      });
    });
  });
}

function deleteEvent() {
  displayEvents();
  rl.question("Enter the number of the event to delete: ", (index) => {
    const eventIndex = parseInt(index) - 1;
    if (eventIndex < 0 || eventIndex >= events.upcoming.length) {
      console.log("Invalid event number.");
      return mainMenu();
    }
    events.upcoming.splice(eventIndex, 1);
    console.log("Event deleted successfully!");
    mainMenu();
  });
}

function markEventCompleted() {
  displayEvents();
  rl.question("Enter the number of the event to mark as completed: ", (index) => {
    const eventIndex = parseInt(index) - 1;
    if (eventIndex < 0 || eventIndex >= events.upcoming.length) {
      console.log("Invalid event number.");
      return mainMenu();
    }

    const event = events.upcoming.splice(eventIndex, 1)[0];
    events.completed.push(event);
    console.log("Event marked as completed!");
    mainMenu();
  });
}

function searchEvents() {
  rl.question("Enter keyword to search: ", (keyword) => {
    const results = [
      ...events.upcoming.filter((e) =>
        e.title.toLowerCase().includes(keyword.toLowerCase())
      ),
      ...events.completed.filter((e) =>
        e.title.toLowerCase().includes(keyword.toLowerCase())
      )
    ];

    console.log("\n--- Search Results ---");
    if (results.length === 0) {
      console.log("No events found.");
    } else {
      results.forEach((event, index) => {
        const status = events.upcoming.includes(event) ? "Upcoming" : "Completed";
        console.log(
          `${index + 1}. ${event.title} (Start: ${event.startTime} | End: ${event.endTime} | Priority: ${event.priority} | Status: ${status} | Reminder: ${event.reminder} minutes before)`
        );
      });
    }
    console.log("\n");
    mainMenu();
  });
}


function mainMenu() {
  console.log("\n--- Event Scheduler ---");
  console.log("1. Display Events");
  console.log("2. Add Event");
  console.log("3. Edit Event");
  console.log("4. Delete Event");
  console.log("5. Mark Event as Completed");
  console.log("6. Search Events");
  console.log("7. Exit");

  rl.question("Enter your choice: ", (choice) => {
    switch (choice) {
      case "1":
        displayEvents();
        mainMenu();
        break;
      case "2":
        addEvent();
        break;
      case "3":
        editEvent();
        break;
      case "4":
        deleteEvent();
        break;
      case "5":
        markEventCompleted();
        break;
      case "6":
        searchEvents();
        break;
      case "7":
        console.log("Exiting Event Scheduler. Goodbye!");
        rl.close();
        break;
      default:
        console.log("Invalid choice. Please try again.");
        mainMenu();
        break;
    }
  });
}

mainMenu();
