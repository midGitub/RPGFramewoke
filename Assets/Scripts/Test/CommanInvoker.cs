using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommanInvoker{
    private List<Command> commands;
    public CommanInvoker() {
        commands = new List<Command>();
    }

    public void AddCommand(Command command) {
        commands.Add(command);
    }

    public void RemoveCommand(Command command) {
        commands.Remove(command);
    }

    public void Invoke() { 
        foreach(Command command in commands){
            command.Execute();
        }
    }
}
