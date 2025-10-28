using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Test
{
    //[InitializeService]
    public class TestInitializeServices
    {
        public TestInitializeServices() 
        {
            Log.Debug("Test initialization");

            // Add services
            World.Services.SetService(typeof(TestService), new TestService(), true);
            World.Services.CreateComponentService(typeof(MeshFilter), typeof(MeshFilter), false);

            // Get services
            var testService = World.GetService<TestService>();
            Log.Debug($"test value: {testService.blah}");

            var meshFilter = World.GetService<MeshFilter>();
            meshFilter.sharedMesh = new Mesh();

            // Make new component service that replaces old one
            World.Services.CreateComponentService(typeof(MeshFilter), typeof(MeshFilter), true);

            if (World.GetService<ILaunchParameters>().TryGetValue("hello", out var p))
            {
                Log.Debug($"Value of hello: {p}");
            }
        }
    }

    public class TestService
    {
        public int blah;
    }
}