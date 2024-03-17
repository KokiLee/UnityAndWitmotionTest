using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class DataParserTests
{
    [UnityTest]
    public IEnumerator ProcessDataCoroutine_UpdatesRollPitchYaw_WithMock()
    {
        // Create a MockSerialHandler instance.
        var mockSerialHandler = new MockSerialHandler();
        // Add test-data.
        byte[] insufficientData = new byte[] { 0x55, 0x53, 0x00, 0x01 };
        mockSerialHandler.EnqueueData(insufficientData);

        // Create a DataParser instance, Inject mockSerialHandler.
        var gameObject = new GameObject();
        var dataParser = gameObject.AddComponent<DataParser>();
        dataParser.serialHandler = mockSerialHandler;

        // Start Coroutine.
        dataParser.StartCoroutine(dataParser.ProcessDataCoroutine());

        // Wait for One frame.
        yield return null;

        // Veryfy that roll, pitch, and yaw have been update.
        Assert.AreEqual(0.0, dataParser.roll, "Roll was not updated.");
        Assert.AreEqual(0.0, dataParser.pitch, "Pitch was not updated.");
        Assert.AreEqual(0.0, dataParser.yaw, "Yaw was not updated.");

        byte[] validTestData = new byte[] { 0x55, 0x53, 0x00, 0x01, 0x00, 0x02, 0x00, 0x03 };
        mockSerialHandler.EnqueueData(validTestData);

        // Restart Coroutine to process new data.
        dataParser.StopCoroutine(dataParser.ProcessDataCoroutine()); // Stop the current coroutine
        dataParser.StartCoroutine(dataParser.ProcessDataCoroutine()); // Start a new instance of the coroutine

        // Wait for another frame to process the new data.
        yield return null;

        // Verify that roll, pitch, and yaw have been updated with valid data.
        Assert.AreNotEqual(0.0, dataParser.roll, "Roll was not updated with valid data.");
        Assert.AreNotEqual(0.0, dataParser.pitch, "Pitch was not updated with valid data.");
        Assert.AreNotEqual(0.0, dataParser.yaw, "Yaw was not updated with valid data.");

        // Destroying an object.
        GameObject.DestroyImmediate(gameObject);
    }
}