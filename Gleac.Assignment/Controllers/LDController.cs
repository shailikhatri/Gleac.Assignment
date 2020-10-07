using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gleac.Assignment.Controllers
{
    [ApiController]
    [Route("api")]

    public class LDController : ControllerBase
    {

        public static int Minimum(int a, int b, int c) => (a = a < b ? a : b) < c ? a : c;

        [Authorize]
        [HttpGet]
        [Route("CalculateLD")]
        public IActionResult CalculateLD(string firstWord, string secondWord)
        {           
            if (String.IsNullOrEmpty(firstWord))
                return BadRequest("First Word cannot be null or empty.");

            if (String.IsNullOrEmpty(secondWord))
                return BadRequest("Second Word cannot be null or empty.");

            if (firstWord.Length>15 || secondWord.Length>15)
                return BadRequest("Invalid Input.");

            Regex r = new Regex("^[a-zA-Z ]*$");
            if(!r.IsMatch(firstWord) || !r.IsMatch(secondWord))
                return BadRequest("Only alphabets are allowed.");

            var n = firstWord.Length + 1;
            var m = secondWord.Length + 1;
            var matrixD = new int[n, m];
            var firstWordArray = new char[n+1];
            var secondWordArray = new char[m + 1];

            const int deletionCost = 1;
            const int insertionCost = 1;

            for (var i = 0; i < n; i++)
            {
                matrixD[i, 0] = i;
            }

            for (var j = 0; j < m; j++)
            {
                matrixD[0, j] = j;
            }

            for (var i = 0; i <= n; i++)
            {
              
                firstWordArray[i] = (i < 2) ? ' ':firstWord.ToCharArray()[i - 2];
            }

            for (var j = 0; j <= m; j++)
            {
                secondWordArray[j] = (j<2)?' ': secondWord.ToCharArray()[j - 2];
            }

           



            for (var i = 1; i < n; i++)
            {
                for (var j = 1; j < m; j++)
                {
                    var substitutionCost = firstWord[i - 1] == secondWord[j - 1] ? 0 : 1;
        

                    matrixD[i, j] = Minimum(matrixD[i - 1, j] + deletionCost, // delete
                                            matrixD[i, j - 1] + insertionCost, // insert
                                            matrixD[i - 1, j - 1] + substitutionCost); // replacement
                }
            }

          


            return Ok(new { distance = matrixD[n - 1, m - 1], firstWordArray = firstWordArray, secondWordArray = secondWordArray, matrix=matrixD}) ;
           
        }

    }
}
