using ImGuiNET;
using Kobra.Rendering;
using Kobra.Scene;
using Kobra.Main;
using Silk.NET.Maths;
using System.Numerics;
using Silk.NET.OpenGL.Extensions.ImGui;

namespace Kobra.Graphics.UI
{
    public class Inspector
    {
        private Mesh? _selectedMesh;
        const float windowWidth = 350f;

        public Mesh? SelectedMesh
        {
            get => _selectedMesh;
            set => _selectedMesh = value;
        }
        public void Draw(KScene scene, Camera camera)
        {
            if (_selectedMesh == null && scene.Meshes.Count > 0)
                _selectedMesh = scene.Meshes[0];

            var io = ImGui.GetIO();

            ImGui.SetNextWindowPos(
                new Vector2(io.DisplaySize.X - windowWidth, 0),
                ImGuiCond.Always
            );

            ImGui.SetNextWindowSize(
                new Vector2(windowWidth, io.DisplaySize.Y),
                ImGuiCond.Always
            );

            if (!ImGui.Begin(
                "Inspector",
                ImGuiWindowFlags.NoMove |
                ImGuiWindowFlags.NoResize |
                ImGuiWindowFlags.NoCollapse
            ))
            {
                ImGui.End();
                return;
            }

            if (scene.Meshes.Count > 0)
            {
                string previewValue = _selectedMesh?.Name ?? "Select Mesh";
                if (ImGui.BeginCombo("Mesh", previewValue))
                {
                    foreach (var mesh in scene.Meshes)
                    {
                        bool isSelected = mesh == _selectedMesh;
                        if (ImGui.Selectable(mesh.Name, isSelected))
                            _selectedMesh = mesh;

                        if (isSelected)
                            ImGui.SetItemDefaultFocus();
                    }
                    ImGui.EndCombo();
                }
            }

            if (_selectedMesh != null)
            {
                ImGui.Text("Transforms");

                var pos = new System.Numerics.Vector3(
                    _selectedMesh.Transform.Position.X,
                    _selectedMesh.Transform.Position.Y,
                    _selectedMesh.Transform.Position.Z
                );
                if (ImGui.DragFloat3("Position", ref pos, 0.1f))
                    _selectedMesh.Transform.Position = new Vector3D<float>(pos.X, pos.Y, pos.Z);

                var rot = new System.Numerics.Vector3(
                    _selectedMesh.Transform.Rotation.X,
                    _selectedMesh.Transform.Rotation.Y,
                    _selectedMesh.Transform.Rotation.Z
                );
                if (ImGui.DragFloat3("Rotation", ref rot, 1f))
                    _selectedMesh.Transform.Rotation = new Vector3D<float>(rot.X, rot.Y, rot.Z);

                var scale = new System.Numerics.Vector3(
                    _selectedMesh.Transform.Scale.X,
                    _selectedMesh.Transform.Scale.Y,
                    _selectedMesh.Transform.Scale.Z
                );
                if (ImGui.DragFloat3("Scale", ref scale, 0.1f))
                    _selectedMesh.Transform.Scale = new Vector3D<float>(scale.X, scale.Y, scale.Z);

                ImGui.Separator();

                ImGui.Text("Material Properties");

                var color = new System.Numerics.Vector3(
                    _selectedMesh.Material.Color.X,
                    _selectedMesh.Material.Color.Y,
                    _selectedMesh.Material.Color.Z
                );
                if (ImGui.ColorEdit3("Color", ref color))
                    _selectedMesh.Material.Color = new Vector3D<float>(color.X, color.Y, color.Z);
            }

            if (SelectedMesh != null)
            {
                var mesh = SelectedMesh;
                var labelPos = mesh.Transform.Position + new Vector3D<float>(0, 0.6f, 0);

                var view = camera.GetViewMatrix();
                var projection = camera.GetProjectionMatrix(1280f / 720f);

                if (Helpers.WorldToScreen(
                    labelPos,
                    view,
                    projection,
                    new Vector2(1280f, 720f),
                    out var screenPos))
                {
                    ImGui.SetNextWindowPos(screenPos, ImGuiCond.Always);
                    ImGui.SetNextWindowBgAlpha(0.35f);

                    ImGui.Begin("Label",
                        ImGuiWindowFlags.NoDecoration | 
                        ImGuiWindowFlags.AlwaysAutoResize | 
                        ImGuiWindowFlags.NoMove | 
                        ImGuiWindowFlags.NoInputs |
                        ImGuiWindowFlags.NoSavedSettings);

                    ImGui.Text($"{mesh.Name}");
                    ImGui.End();
                }

                
            }

            ImGui.End();
        }
    }
}

