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

/// <summary>
/// Attaches a CommandBuffer to the required Camera OnEnable that clears the 
/// Camera's target to Red.
/// </summary>
[RequireComponent(typeof(Camera))]
public class ClearToRedCommandBuffer : MonoBehaviour
{
    // NOTE: Slightly modified from the example in the post for good practice.

    CommandBuffer commandBuffer;
    CameraEvent cameraEvent = CameraEvent.AfterEverything;

    private void OnEnable()
    {
        commandBuffer = new CommandBuffer()
        {
            name = "Clear to Red"
        };
        commandBuffer.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
        commandBuffer.ClearRenderTarget(true, true, Color.red, 1f);
        GetComponent<Camera>().AddCommandBuffer(cameraEvent, commandBuffer);
    }

    private void OnDisable()
    {
        GetComponent<Camera>().RemoveCommandBuffer(cameraEvent, commandBuffer);
        commandBuffer.Dispose();
    }
}
