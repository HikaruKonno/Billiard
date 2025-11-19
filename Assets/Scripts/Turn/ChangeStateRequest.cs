using System;
using System.Collections.Generic;


public class ChangeStateRequest
{
    public Func<IState> Factory { get; }

    public  ChangeStateRequest(Func<IState> factory)
    {
        Factory = factory;
    }
}

