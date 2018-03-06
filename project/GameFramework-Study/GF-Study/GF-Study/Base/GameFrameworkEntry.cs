
using System;
using System.Collections.Generic;

public static class GameFrameworkEnty
{
    private const string GameFrameworkVersion = "3.1.0";
    private static readonly LinkedList<GameFrameworkModule> s_GameFrameworkModules = new LinkedList<GameFrameworkModule>();

    public static string Version {
        get { return GameFrameworkVersion; }
    }

    static void Update() {
        foreach (GameFrameworkModule module in s_GameFrameworkModules)
        {
            module.Update();
        }
    }
    static void Shutdown() {
        for (LinkedListNode<GameFrameworkModule> current = s_GameFrameworkModules.Last; current != null; current = current.Previous)
        {
            current.Value.Shutdown();
        }

        s_GameFrameworkModules.Clear();
        ReferencePool.ClearAll();
        Log.SetLogHelper(null);

    }
    public static T GetModule<T>() where T : class
    {
        Type interfaceType = typeof(T);
        if (!interfaceType.IsInterface)
        {
            throw;
        }

        if (!interfaceType.FullName.StartsWith("GameFramework"))
        {
            throw;
        }

        string moduleName = string.Format("{0}.{1]", interfaceType.Namespace, interface.Name.ToString(1));
        Type moduleType = Type.GetType(moduleName);
        if (moduleType == null) {
            throw ;
        }

        return GetModule(moduleType) as T;
    }
    private static GameFrameworkModule GetModule(Type moduleType)
    {
        foreach (GameFrameworkModule module in s_GameFrameworkModules)
        {
            if (module.GetType() == moduleType)
            {
                return module;
            }
        }

        return CreateModule(moduleType);
    }
    static GameFrameworkModule CreateModule(Type moduleType) {
        GameFrameworkModule module = (GameFrameworkModule)Activator.CreateInstance(moduleType);

        LinkedListNode<GameFrameworkModule> current = s_GameFrameworkModules.First;
        while (current != null)
        {
            if (module.Priority > current.Value.Priority)
            {
                break;
            }

            current = current.Next;
        }

        if (current != null)
        {
            s_GameFrameworkModules.AddBefore(current, module);
        }
        else
        {
            s_GameFrameworkModules.AddLast(module);
        }

        return module;
    }
}

class GameFrameworkModule { }

class TaskPool { }