using System;
using System.Collections.Generic;
using System.Linq;
using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using static System.Math;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;

namespace Fusee.Tutorial.Core
{
    public class AssetsPicking : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRenderer _sceneRenderer;
        private ScenePicker _scenePicker;
        private float _camAngle = 0;
        private float _zoom = 40;
        private float _movex = 0;
        private float _movey = 0;

        private TransformComponent _baseTransform;
        private PickResult _currentPick;
        private float3 _oldColor;
        private TransformComponent _radRechts1Transform;
        private TransformComponent _radRechts2Transform;
        private TransformComponent _radRechts3Transform;
        private TransformComponent _radRechts4Transform;
        private TransformComponent _radRechts5Transform;
        private TransformComponent _radRechts6Transform;
        private TransformComponent _radRechts7Transform;
        private TransformComponent _radRechts8Transform;

        private TransformComponent _radLinks1Transform;
        private TransformComponent _radLinks2Transform;
        private TransformComponent _radLinks3Transform;
        private TransformComponent _radLinks4Transform;
        private TransformComponent _radLinks5Transform;
        private TransformComponent _radLinks6Transform;
        private TransformComponent _radLinks7Transform;
        private TransformComponent _radLinks8Transform;

        private TransformComponent _kabineTransform;
        private TransformComponent _kanoneTransform;

        SceneContainer CreateScene()
        {
            // Initialize transform components that need to be changed inside "RenderAFrame"
            _baseTransform = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 0, 0)
            };

            // Setup the scene graph
            return new SceneContainer
            {
                Children = new List<SceneNodeContainer>
                {
                    new SceneNodeContainer
                    {
                        Components = new List<SceneComponentContainer>
                        {
                            // TRANSFROM COMPONENT
                            _baseTransform,

                            // MATERIAL COMPONENT
                            new MaterialComponent
                            {
                                Diffuse = new MatChannelContainer { Color = new float3(0.7f, 0.7f, 0.7f) },
                                Specular = new SpecularChannelContainer { Color = new float3(1, 1, 1), Shininess = 5 }
                            },

                            // MESH COMPONENT
                            // SimpleAssetsPickinges.CreateCuboid(new float3(10, 10, 10))
                            SimpleMeshes.CreateCuboid(new float3(10, 10, 10))
                        }
                    },
                }
            };
        }

        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to white (100% intentsity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0.8f, 0.9f, 0.7f, 1);

            _scene = AssetStorage.Get<SceneContainer>("Tank.fus");

            _radRechts1Transform = _scene.Children.FindNodes(node => node.Name == "Reifen_R.001")?.FirstOrDefault()?.GetTransform();
            _radRechts2Transform = _scene.Children.FindNodes(node => node.Name == "Reifen_R.002")?.FirstOrDefault()?.GetTransform();
            _radRechts3Transform = _scene.Children.FindNodes(node => node.Name == "Reifen_R.003")?.FirstOrDefault()?.GetTransform();
            _radRechts4Transform = _scene.Children.FindNodes(node => node.Name == "Reifen_R.004")?.FirstOrDefault()?.GetTransform();
            _radRechts5Transform = _scene.Children.FindNodes(node => node.Name == "Reifen_R.005")?.FirstOrDefault()?.GetTransform();
            _radRechts6Transform = _scene.Children.FindNodes(node => node.Name == "Reifen_R.006")?.FirstOrDefault()?.GetTransform();
            _radRechts7Transform = _scene.Children.FindNodes(node => node.Name == "Reifen_R.007")?.FirstOrDefault()?.GetTransform();
            _radRechts8Transform = _scene.Children.FindNodes(node => node.Name == "Reifen_R.008")?.FirstOrDefault()?.GetTransform();

            _radLinks1Transform = _scene.Children.FindNodes(node => node.Name == "Reifen_L.001")?.FirstOrDefault()?.GetTransform();
            _radLinks2Transform = _scene.Children.FindNodes(node => node.Name == "Reifen_L.002")?.FirstOrDefault()?.GetTransform();
            _radLinks3Transform = _scene.Children.FindNodes(node => node.Name == "Reifen_L.003")?.FirstOrDefault()?.GetTransform();
            _radLinks4Transform = _scene.Children.FindNodes(node => node.Name == "Reifen_L.004")?.FirstOrDefault()?.GetTransform();
            _radLinks5Transform = _scene.Children.FindNodes(node => node.Name == "Reifen_L.005")?.FirstOrDefault()?.GetTransform();
            _radLinks6Transform = _scene.Children.FindNodes(node => node.Name == "Reifen_L.006")?.FirstOrDefault()?.GetTransform();
            _radLinks7Transform = _scene.Children.FindNodes(node => node.Name == "Reifen_L.007")?.FirstOrDefault()?.GetTransform();
            _radLinks8Transform = _scene.Children.FindNodes(node => node.Name == "Reifen_L.008")?.FirstOrDefault()?.GetTransform();

            _kabineTransform = _scene.Children.FindNodes(node => node.Name == "Kabine")?.FirstOrDefault()?.GetTransform();
            _kanoneTransform = _scene.Children.FindNodes(node => node.Name == "Kanone")?.FirstOrDefault()?.GetTransform();

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRenderer(_scene);
            _scenePicker = new ScenePicker(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {


            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            if (Mouse.RightButton)
            {
                _camAngle += 0.5f * Mouse.Velocity.x;
            }

            if (Keyboard.IsKeyDown(KeyCodes.Up))
            {
                _zoom += 5f;
               
            }

            if (Keyboard.IsKeyDown(KeyCodes.Left))
            {
                _movex -= 5f;
            }

            if (Keyboard.IsKeyDown(KeyCodes.Right))
            {
                _movex += 5f;
            }

            if (Keyboard.IsKeyDown(KeyCodes.Down))
            {
                _zoom -= 5f;
            }

            if (Keyboard.IsKeyDown(KeyCodes.NumPad8))
            {
                _movey += 5f;
            }

            if (Keyboard.IsKeyDown(KeyCodes.NumPad2))
            {
                _movey -= 5f;
            }

            if(_currentPick != null)
            {
                if (_currentPick.Node.Name == "Reifen_R.001")
                {
                    float rrRot = _radRechts1Transform.Rotation.x;
                    rrRot += 0.1f * Keyboard.ADAxis;
                    _radRechts1Transform.Rotation = new float3(rrRot, 0, 0);
                }

                if (_currentPick.Node.Name == "Reifen_R.002")
                {
                    float rrRot = _radRechts2Transform.Rotation.x;
                    rrRot += 0.1f * Keyboard.ADAxis;
                    _radRechts2Transform.Rotation = new float3(rrRot, 0, 0);
                }

                if (_currentPick.Node.Name == "Reifen_R.003")
                {
                    float rrRot = _radRechts3Transform.Rotation.x;
                    rrRot += 0.1f * Keyboard.ADAxis;
                    _radRechts3Transform.Rotation = new float3(rrRot, 0, 0);
                }

                if (_currentPick.Node.Name == "Reifen_R.004")
                {
                    float rrRot = _radRechts4Transform.Rotation.x;
                    rrRot += 0.1f * Keyboard.ADAxis;
                    _radRechts4Transform.Rotation = new float3(rrRot, 0, 0);
                }

                if (_currentPick.Node.Name == "Reifen_R.005")
                {
                    float rrRot = _radRechts5Transform.Rotation.x;
                    rrRot += 0.1f * Keyboard.ADAxis;
                    _radRechts5Transform.Rotation = new float3(rrRot, 0, 0);
                }

                if (_currentPick.Node.Name == "Reifen_R.006")
                {
                    float rrRot = _radRechts6Transform.Rotation.x;
                    rrRot += 0.1f * Keyboard.ADAxis;
                    _radRechts6Transform.Rotation = new float3(rrRot, 0, 0);
                }

                if (_currentPick.Node.Name == "Reifen_R.007")
                {
                    float rrRot = _radRechts7Transform.Rotation.x;
                    rrRot += 0.1f * Keyboard.ADAxis;
                    _radRechts7Transform.Rotation = new float3(rrRot, 0, 0);
                }

                if (_currentPick.Node.Name == "Reifen_R.008")
                {
                    float rrRot = _radRechts8Transform.Rotation.x;
                    rrRot += 0.1f * Keyboard.ADAxis;
                    _radRechts8Transform.Rotation = new float3(rrRot, 0, 0);
                }


                if (_currentPick.Node.Name == "Reifen_L.001")
                {
                    float rrRot = _radLinks1Transform.Rotation.x;
                    rrRot += 0.1f * Keyboard.ADAxis;
                    _radLinks1Transform.Rotation = new float3(rrRot, 0, 0);
                }

                if (_currentPick.Node.Name == "Reifen_L.002")
                {
                    float rrRot = _radLinks2Transform.Rotation.x;
                    rrRot += 0.1f * Keyboard.ADAxis;
                    _radLinks2Transform.Rotation = new float3(rrRot, 0, 0);
                }

                if (_currentPick.Node.Name == "Reifen_L.003")
                {
                    float rrRot = _radLinks3Transform.Rotation.x;
                    rrRot += 0.1f * Keyboard.ADAxis;
                    _radLinks3Transform.Rotation = new float3(rrRot, 0, 0);
                }

                if (_currentPick.Node.Name == "Reifen_L.004")
                {
                    float rrRot = _radLinks4Transform.Rotation.x;
                    rrRot += 0.1f * Keyboard.ADAxis;
                    _radLinks4Transform.Rotation = new float3(rrRot, 0, 0);
                }

                if (_currentPick.Node.Name == "Reifen_L.005")
                {
                    float rrRot = _radLinks5Transform.Rotation.x;
                    rrRot += 0.1f * Keyboard.ADAxis;
                    _radLinks5Transform.Rotation = new float3(rrRot, 0, 0);
                }

                if (_currentPick.Node.Name == "Reifen_L.006")
                {
                    float rrRot = _radLinks6Transform.Rotation.x;
                    rrRot += 0.1f * Keyboard.ADAxis;
                    _radLinks6Transform.Rotation = new float3(rrRot, 0, 0);
                }

                if (_currentPick.Node.Name == "Reifen_L.007")
                {
                    float rrRot = _radLinks7Transform.Rotation.x;
                    rrRot += 0.1f * Keyboard.ADAxis;
                    _radLinks7Transform.Rotation = new float3(rrRot, 0, 0);
                }

                if (_currentPick.Node.Name == "Reifen_L.008")
                {
                    float rrRot = _radLinks8Transform.Rotation.x;
                    rrRot += 0.1f * Keyboard.ADAxis;
                    _radLinks8Transform.Rotation = new float3(rrRot, 0, 0);
                }

                if (_currentPick.Node.Name == "Kabine")
                {
                    float rrRot = _kabineTransform.Rotation.y;
                    rrRot += 0.1f * Keyboard.ADAxis;
                    _kabineTransform.Rotation = new float3(0, rrRot, 0);
                }

                if (_currentPick.Node.Name == "Kanone")
                {

                    float rrRot = _kanoneTransform.Rotation.x;
                    if (Keyboard.IsKeyDown(KeyCodes.W))
                    {


                        if (_kanoneTransform.Rotation.x >= -0.2f && _kanoneTransform.Rotation.x <= 0.2f)
                        {
                            rrRot += 0.1f;
                            _kanoneTransform.Rotation = new float3(rrRot, 0, 0);
                        }
                    }

                    if (Keyboard.IsKeyDown(KeyCodes.S))
                    {
                        if (_kanoneTransform.Rotation.x <= 0.4f && _kanoneTransform.Rotation.x > -0.1f)
                        {
                            rrRot -= 0.1f;
                            _kanoneTransform.Rotation = new float3(rrRot, 0, 0);
                        }
                    }
                }

            }


            // Setup the camera 
            RC.View = float4x4.CreateTranslation(_movex, _movey, _zoom) * float4x4.CreateRotationY(_camAngle / 2000);
           



            float2 pickPosClip = Mouse.Position * new float2(2.0f / Width, -2.0f / Height) + new float2(-1, 1);
            _scenePicker.View = RC.View;
            _scenePicker.Projection = RC.Projection;
            List<PickResult> pickResults = _scenePicker.Pick(pickPosClip).ToList();
            PickResult newPick = null;


                if (pickResults.Count > 0)
                {
                    pickResults.Sort((a, b) => Sign(a.ClipPos.z - b.ClipPos.z));
                    newPick = pickResults[0];
                    
                }

                if (Mouse.LeftButton) {

               
                if (newPick?.Node != _currentPick?.Node)
                    {

                        if (_currentPick != null)
                        {
                            _currentPick.Node.GetMaterial().Diffuse.Color = _oldColor;
                           


                        }
                        if (newPick != null)
                        {
                            var mat = newPick.Node.GetMaterial();
                            _oldColor = mat.Diffuse.Color;
                            mat.Diffuse.Color = new float3(1, 0.4f, 0.4f);
                        }
                        _currentPick = newPick;
                    }
                }
          

                

           

            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered farame) on the front buffer.
            Present();
        }


        // Is called when the window was resized
        public override void Resize()
        {
            // Set the new rendering area to the entire new windows size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width / (float)Height;

            // 0.25*PI Rad -> 45° Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }
    }
}
