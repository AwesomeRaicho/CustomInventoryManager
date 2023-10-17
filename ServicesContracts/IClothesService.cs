using Microsoft.VisualBasic.FileIO;
using ServicesContracts.DTO;
using ServicesContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesContracts
{
    public interface IClothesService
    {
        /// <summary>
        /// Add new Clothes to DB
        /// </summary>
        /// <param name="clothesAddRequest">ClothesAddRequest object</param>
        /// <returns>ClothesResponse object</returns>
        ClothesResponse AddClothes(ClothesAddRequest? clothesAddRequest);

        /// <summary>
        /// Get all Clothes in DB
        /// </summary>
        /// <returns>All clothes in DB</returns>
        List<ClothesResponse> GetAllClothes();

        /// <summary>
        /// Get clothes with ClothedID (Guid)
        /// </summary>
        /// <param name="guid">guid to search and return in DB</param>
        /// <returns>ClothesResponse object</returns>
        ClothesResponse? GetClothesByClothesID(Guid? guid);

        /// <summary>
        /// get all sold clothes from sold DB table
        /// </summary>
        /// <returns>List<ClothesResponse> object List</returns>
        List<ClothesResponse> GetAllSoldClothes();

        /// <summary>
        /// removes clothes from the DB and adds it to the "sold" DB table
        /// </summary>
        /// <param name="guid">Guid id to remove and add to sold DB</param>
        /// <returns>true or false if the Clothes was properly changed tables</returns>
        bool SoldClothesByClothesID(Guid? guid);

        /// <summary>
        /// Delete clothes from the DB
        /// </summary>
        /// <param name="guid">guid id to remove</param>
        /// <returns>true or false if properly removed from main DB
        /// </returns>
        bool DeleteClothes(Guid? guid);

        /// <summary>
        /// Get a list of the clothes filtered by matching string one of its properties
        /// </summary>
        /// <param name="sortBy">property to filter by</param>
        /// <param name="sortString">search string that should match with the property</param>
        /// <returns>List<ClothesResponse> Object List</returns>
        List<ClothesResponse> GetFilteredClothes(string filterBy, string? filterString);

        /// <summary>
        /// sorts List provided by specified property
        /// </summary>
        /// <returns></returns>
        List<ClothesResponse> GetSortedClothes(List<ClothesResponse> allClothes, string sortBy, SortOrderOptions sortOrder);

        /// <summary>
        /// update an existing Clothes in DB 
        /// </summary>
        /// <param name="clothesUpdateRequest">ClothesUpdateRequest object </param>
        /// <returns>ClothesResponse object</returns>
        ClothesResponse UpdateClothes(ClothesUpdateRequest? clothesUpdateRequest);
    }
}
