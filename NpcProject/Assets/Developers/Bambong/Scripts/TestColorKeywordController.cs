using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestColorKeywordController : KeywordController
{
    [SerializeField]
    private int id;

    public int Id { get => id; }

    public override bool HandleObjectKeyword(KeywordController objectKeywordController)
    {

        switch (objectKeywordController.KeywordId) 
        {
            case "TestColorKeywordController":
                TestColorKeywordController colorKeywordController = (TestColorKeywordController)objectKeywordController;
                return colorKeywordController.id == Id;

            default:
                return false;
        }
    }
}
