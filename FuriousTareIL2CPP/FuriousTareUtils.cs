using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FuriousTareIL2CPP;

public static class FuriousTareUtils
{
    public static string GetFullPath(GameObject gameObject)
    {
        var pathParts = new List<string>();
        while (gameObject)
        {
            pathParts.Add(
                gameObject.name
            );
            if (gameObject.transform.parent)
            {
                gameObject = gameObject.transform.parent.gameObject;
            }
            else
            {
                gameObject = null;
            }
        }

        var sb = new StringBuilder();
        for (var i = pathParts.Count - 1; i >= 0; i--)
        {
            sb.Append(
                pathParts[i]
            );
            if (i > 0)
            {
                sb.Append(
                    "/"
                );
            }
        }

        return sb.ToString();
    }
}
