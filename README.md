# Unity-Easy-Update-Controller
A easy controller to update data,model or other files in unity application,it's very basic,not complex!

# Main Script : UpdateController.cs
To control update active,compare UpdateTable to see it needs to update!
if you want to update new data,then rewarite UpdateTable,make sure the version data is changed.
In InitControler.cs ,you just call startUpdate function,the update will active.
the delegate function OnUpdateOver will return the success data to you.

# Main Update Table : UpdateTalble.txt
All need update data will tag in this file,and the version of a file will make sense in update action.

# Sample
To drag InitController and UpdateController to a unity gameobject,Then run.The Console will show the process.
