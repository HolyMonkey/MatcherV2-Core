using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Matcher.ExpressionGenerator;

public class ExpressionGeneratoTests {

	[Test]
	public void OnePlusGeneratorTest() {

        Tree tree = new Tree();
        for (int i = 0; i < 100; i++)
        {
            string expression = tree.Generate(1, 0, 0, 0, 1);
            Assert.True(expression.Contains("+") && !(expression.Contains("-") || expression.Contains("*") || expression.Contains("/")), expression);
        }
	}
}
