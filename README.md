# Office-Tracker

Der **Office-Tracker** wurde neben meiner aktuellen Umschulung zum Fachinformatiker Anwendungsentwicklung als kleines Nebenprojekt angefangen, damit meine anderen Mitschüler als auch meine Wenigkeit ein kleines Hilfsprogramm haben um unsere Anwesenheitsquote im Blick zu haben und um diese zu planen mit Berechnungen für die kommenden x-Wochen. Der Plan mit dieser Desktop-Anwendung ist es, dies stetig weiterzuentwickeln und es als Open-Source-Projekt anzubieten, damit auch andere die Lust drauf haben zu unterstützen mit Ideen & Co. um es auch noch außerhalb meiner aktuellen Umschulung anbieten zu können. 

## ✨ Features im Überblick

### 🚀 Schneller Start mit dem Setup-Assistenten
Beim ersten Start führt dich ein einfacher Assistent durch die paar notwendigen Schritte durch, um Anfangsdaten zu haben, mit dem die Anwendung arbeiten kann. Du wirst ein Namen angeben können, deine Tage an denen du standardmäßig HomeOffice hast (Mo. - Fr.) sowie deine Ziel-Quote die du mindestens/höchstens haben kannst, ohne in Schwierigkeiten zu kommen. Am Ende bekommt man noch eine kleine Zusammenfassung und kann seine angegebenen Daten nochmals überprüfen & gegebenenfalls auch zu den Schritten zurückkehren zm noch Änderungen vornehmen zu können.

![Setup-Assistent Start](Assets/Screenshots/wizard.png)

### 🗓️ Tägliche Anwesenheit erfassen
Trage ganz einfach jeden Tag maximal 1x ein, ob du dich im HomeOffice oder am Standort/Büro befindest. Dazu reichen 2 einfache Klicks aus - fertig!

![Setup-Assistent Start](Assets/Screenshots/add_current_day.png)

### 📊 Anwesenheits-Statistiken
Im oberen Bereich der Anwendung hast du jederzeit deine aktuelle Anwesenheits-Statistik im Auge und siehst mit einem Blick, wie das Verhältnis zu deiner Zielquote ausschaut.

![Setup-Assistent Start](Assets/Screenshots/stats_overview.png)

### 🔮 Wochen-Vorhersage (Statistiken)
Basierend auf deine aktuelle Anwesenheits-Statistik und mit deinen geplanten Tagen, berechnet die Anwendung eine Art Prognose für die kommenden Wochen aus. So siehst du frühzeitig und ohne selbst rechnen zu müssen, wann deine Zielquote wieder erreicht ist. Weitere Details und Angaben sind hier bereits geplant.

![Setup-Assistent Start](Assets/Screenshots/calculated_weeks.png)

### 📝 Tage planen
Du hast die Möglichkeit wie oben bereits kurz angeschnitten - Tage zu planen. Trage im vorraus bereits ein ob du einen standardmäßigen HomeOffice Tag zu einem Standort/Büro Tag - oder andersrum machst. Dies soll dabei helfen deine Planung hinsichtlich zu deiner Zielquote besser zu managen ohne viel Stress den man eh zu genüge im Alltag bereits hat.

![Setup-Assistent Start](Assets/Screenshots/plannable_days.png)

### 💾 Lokale Datenspeicherung
Die Daten werden auf keinen externen Server gespeichert sondern ist mit einer SQL-Lite Datenbank versehen. Keine Cloud oder sonstiges - alle Informationen sind also zu 100% unter deiner Kontrolle. Du brauchst einen neuen Datensatz? Kein Problem - einfach die Data-Bash Datei entfernen/verschieben und das Programm führt dich erneut durch den Assistenten.

## 🛠️ Installation

1.  Laden die neueste Version vom Office-Tracker für dein Betriebssystem von den [Releases](https://github.com/JumpSpinn/gfn-office-tracker/releases) herunter.
    * `OfficeTracker-win-x64.exe` für Windows
    * `OfficeTracker-linux-x64` / `.zip` / `.tar.gz` für Linux
2.  Entpacken ggbf. die heruntergeladene Datei (falls es eine .zip/.tar.gz ist).
4.  Führe die `OfficeTracker.exe` (Windows) oder die `OfficeTracker` ausführbare Datei (Linux) aus.

---

## 🚀 Zukünftig geplante Features:

* Datenbank-Speicherort ändern (Standardpfad in die AppData)
	* Speicherort auswählbar gestalten
* Eine Option, um das Ganze auch als Zeiterfassung nutzen zu können.
* Management-Option, damit man über Teamauswertungen sehen kann, wie groß die Quote HomeOffice/Office pro Team ist.
* Möglichkeit der Angabe, auch am Wochenende zu arbeiten.
* Einstellungsmöglichkeiten

---

## 📄 Lizenz

Dieses Projekt ist unter der MIT-Lizenz lizenziert - siehe die [LICENSE.md](LICENSE.md)-Datei für Details.

---
