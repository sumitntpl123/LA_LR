# Phonetic Bengali input for Windows Forms application
This project adds Windows Froms controls for **Bengali phonetic mode**(where user types in English letter which sounds similar to Bengali)

Two user controls are made available through this repository
1. `PBITextBox` - Enter Text in English or Bengali phonetic mode
2. `PBIComboBox` - Search and select from a list of options. The search happens in either English or Bengali phonetic mode

Two packages are published in this [release](https://github.com/NevaehTechIncubation/PhoneticBengaliInput-WinForms/releases/latest)
1. `PhoneticBengaliInput.0.1.0.nupkg` - nuget package for the user controls
2. `AvroPhoneticBengaliInput.0.1.0.nupkg` - nuget package for the phonetic to Bengali converter, based on Avro Keyboard implementation

You need to add these two packages to your project. Additionally add this line to initialize the phonetic to Bengali converter somewhere in your form's constructor.
```cs
AvroBengaliPhoneticParser.Parser.Register();
```
- You can set the initial language of the controls. By default it is English.
- You can customize the language switcher button. You can make it visible or invisible. You can also change its shortcut key, by default <kbd>Ctrl</kbd> + <kbd>E</kbd>
- There are some events for you to latch on, they starts with `Inner` prefix mostly
