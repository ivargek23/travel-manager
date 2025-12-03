using BL.BLModels;
using BL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IRepositories
{
    public interface IImageRepository
    {
        public IEnumerable<BLImage> GetImages();
        public BLImage GetImage(int id);
        public BLImage SaveImage(BLImage blImage);
        public BLImage UpdateImage(int id, BLImage blImage);
        public Image DeleteImage(int id);
    }
}
