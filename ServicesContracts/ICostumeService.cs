
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface ICostumeService
    {
        /// <summary>
        /// Adds a costume to DB
        /// </summary>
        /// <param name="costumeAddRequest">CostumeAddRequest object</param>
        /// <returns>CostumeResponse</returns>
        CostumeResponse AddCostume(CostumeAddRequest? costumeAddRequest);

        /// <summary>
        /// Gets all costumes in DB
        /// </summary>
        /// <returns>List of CostumeResponse</returns>
        List<CostumeResponse> GetAllCostumes();

        /// <summary>
        /// Gets costume by the Guid ID provided
        /// </summary>
        /// <param name="costumeID">CostumeID of the costume</param>
        /// <returns>CostumeResponse that be</returns>
        CostumeResponse? GetCostumeByCostumeID(Guid? costumeID);

        List<CostumeResponse> GetAllSoldCostumes();

        /// <summary>
        /// Removes a costume and sents it to sold history DB
        /// </summary>
        /// <param name="costumeID">costume ID to remove</param>
        /// <returns>bool with true if successfully removed of false if removal failed</returns>
        bool SoldCostumeByCostumeID(Guid? costumeID);

        /// <summary>
        /// Deletes costume completly from the DB
        /// </summary>
        /// <param name="costumeID">CostumeID to delete</param>
        /// <returns>bool with true if  deleted successfully or false if unable to delete</returns>
        bool DeleteCostume(Guid? costumeID);

        //required Methods
        // * filtered List
        // * sorted List

        //update costume will need a DTO
        //CostumeResponse UpdateCostume();
    }
}
