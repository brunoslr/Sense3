using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;

public class ObjectPoolingTests {


    GameObject[] goArray;

    [TestFixtureSetUp]
    public void OneTimeSetup()
    {
        TestGameObjectArray();
    }

    [Test]
	public void TestPoolObject() {
        //Arrange
        var objPoolMock = GetObjectPoolMock();
        var sut = GetObjectPoolControllerMock(objPoolMock);

        sut.SetObjectPrefabs(goArray);
        sut.InitializeObjectPool();


        //Act
        //Try to rename the GameObject

        var gameObjectFromPool = sut.GetObjectAtIndexPrefab(0, true);

        //Assert
        //The object has a new name
        Assert.AreEqual(goArray[0], gameObjectFromPool );
	}


    private void TestGameObjectArray()
    {
        goArray = new GameObject[1];
        goArray[0] = new GameObject();
        goArray[0].name = "Teste";
        
    }

    private IObjectPoolController GetObjectPoolMock()
    {
        var controller = Substitute.For<IObjectPoolController>();    
        controller.ReturnNewInstanceOf(new GameObject()).ReturnsForAnyArgs(goArray[0]);
        return controller;        
    }

    private ObjectPoolController GetObjectPoolControllerMock(IObjectPoolController objectPoolController)
    {
        var controller = Substitute.For<ObjectPoolController>();
        controller.SetObjectPoolController(objectPoolController);
        return controller;
    }

}
