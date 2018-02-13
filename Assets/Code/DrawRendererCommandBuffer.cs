/**
* MIT License
* 
* Copyright (c) 2018 Joseph Pasek
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
**/

using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CommandBuffer))]
public class DrawRendererCommandBuffer : MonoBehaviour
{
    new Camera camera;

    CommandBuffer commandBuffer;
    [SerializeField]
    CameraEvent cameraEvent = CameraEvent.BeforeForwardOpaque;

    RenderTexture renderTexture;
    RenderTargetIdentifier rtID;

    [SerializeField]
    Renderer targetRenderer;

    [SerializeField]
    int subMeshID;
    [SerializeField]
    int shaderPass;

    public bool debug = true; 

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        // Create Render Target
        if(renderTexture == null)
        {
            renderTexture = new RenderTexture(camera.pixelWidth, camera.pixelHeight, 0)
            {
                name = "DrawRendererTarget"
            };
            renderTexture.Create();
        }
        rtID = new RenderTargetIdentifier(renderTexture);

        // Build or clear the CommandBuffer
        if (commandBuffer == null)
        {
            commandBuffer = new CommandBuffer()
            {
                name = "Draw Renderer"
            };
        }
        else
            commandBuffer.Clear();

        commandBuffer.SetRenderTarget(rtID);
        commandBuffer.ClearRenderTarget(true, true, Color.clear, 1f);

        if (targetRenderer != null)
            commandBuffer.DrawRenderer(
                targetRenderer, 
                targetRenderer.sharedMaterial, 
                subMeshID, 
                shaderPass);
        else
            Debug.LogWarning("No Renderer was assigned, the target RenderTexture will be empty.", this);

        camera.AddCommandBuffer(cameraEvent, commandBuffer);
    }

    private void OnDisable()
    {
        // Clean up
        camera.RemoveCommandBuffer(cameraEvent, commandBuffer);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(debug ? renderTexture : src, dest);
    }

    /// <summary>
    /// Debug function to select the RenderTexture
    /// </summary>
    [ContextMenu("Select RenderTexture")]
    private void SelectRenderTexture()
    {
#if UNITY_EDITOR
        if(renderTexture != null)
            UnityEditor.Selection.activeObject = renderTexture;
#endif
    }

}
