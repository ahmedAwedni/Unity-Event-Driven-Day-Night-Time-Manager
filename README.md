# Unity Event-Driven Day/Night & Time Manager

A highly decoupled, event-driven Time Management system for Unity. Perfect for RPGs, farming simulators, and open-world games. It handles internal time ticking independently from visual rendering, ensuring your codebase remains clean and highly performant.

---

## ✨ Features

* **Decoupled Architecture:** The "TimeManager" strictly handles the mathematics of time (Minutes, Hours, Days) and broadcasts C# events. It knows nothing about lights or graphics.
* **Dual Celestial Bodies:** Supports independent Sun and Moon directional lights. The moon stays perfectly opposite to the sun, casting beautiful cool shadows during the night phase.
* **Smooth Day/Night Cycle:** The "DayNightController" passively listens to the time and calculates smooth celestial rotations. It uses Unity "Gradients" and "AnimationCurves" to let artists fully control the color and intensity of both the sun and moon at any given hour.
* **Event-Driven Triggers:** The system includes a "TimeEventTrigger" component. Easily hook up UnityEvents to trigger exactly at a specific in-game time (e.g., street lamps turning on at 18:00, or a shop door unlocking at 08:00).
* **Adjustable Timescale:** Control exactly how many real-world seconds equal one in-game hour directly from the inspector.

---

## 🧠 Design Notes

A common mistake in beginner Day/Night cycles is putting lighting, rotation, and time-ticking all into one massive "Update()" loop. This makes it incredibly difficult to sync NPC schedules or UI clocks to the sun.

This system relies heavily on the **Observer Pattern**. The "TimeManager" acts as the global clock. Every in-game minute, it shouts out "The time is now 14:30!". 
* The "DayNightController" hears this and updates the sun and moon. 
* The "TimeEventTrigger" hears this and checks if it should turn on a lightbulb. 
* Your UI Canvas can listen to this to update the text on the player's minimap clock. 
Everything stays perfectly synced without any messy spaghetti code or heavy "FindObjectOfType" calls.

---

## 📂 Included Scripts

* "TimeManager.cs" - The core Singleton that calculates time based on "Time.deltaTime" and fires off global C# Actions.
* "DayNightController.cs" - Evaluates time as a normalized float (0.0 to 1.0) to smoothly interpolate Directional Light rotations, colors, and intensities for both a Sun and a Moon.
* "TimeEventTrigger.cs" - A utility script that fires a "UnityEvent" when a specific Hour and Minute is reached.

---

## 🧩 How To Use

1. **Setup the Manager:** Create an empty GameObject named "TimeManager" and attach the "TimeManager.cs" script. Set your starting hour and the real-time speed.
2. **Setup the Celestial Bodies:** Create two Directional Lights in your scene (name them "Sun" and "Moon"). Attach the "DayNightController.cs" script to a manager object and slot both lights into their respective fields.
3. **Configure Lighting:** * **Sun:** Set the Gradient edges to dark orange/black for dawn/dusk, and the center to bright white for noon. Set the Intensity curve to peak in the middle and drop to 0 at the edges.
   * **Moon:** Set the Gradient edges to cool blue for midnight, and the center to black. Set the Intensity curve to peak at the edges (0.0 and 1.0) and drop to 0 in the middle (0.5).
4. **Create a Scheduled Event:** Select a GameObject in your scene (like a point light). Attach the "TimeEventTrigger.cs" script. Set the trigger time to 19:00, and drag the point light's "SetActive" method into the UnityEvent box to turn it on at night.

---

## 🚀 Possible Extensions

* **Calendar System:** Expand the "TimeManager" to track Months, Years, and Seasons. Fire an "OnSeasonChanged" event to swap out tree materials from Green to Snowy.
* **Save/Load Integration:** Implement an "ISaveable" interface on the "TimeManager" to serialize the "CurrentMinute", "CurrentHour", and "CurrentDay" so time persists between play sessions.
* **Weather System Integration:** Create a separate script that listens to the time and randomly triggers rain or fog. The DayNightController could then read that weather state to dynamically lower the light intensity and add grey tints to the color gradients on cloudy days.

---

## 🛠 Unity Version

Tested in Unity6+ (should work without any problems in newer versions).

---

## 📜 License

MIT
