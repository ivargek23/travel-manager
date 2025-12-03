using AutoMapper;
using BL.BLModels;
using BL.IRepositories;
using BL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BL.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        public ImageRepository(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Models.Image DeleteImage(int id)
        {
            throw new NotImplementedException();
        }

        public BLImage GetImage(int id)
        {
            var image = _context.Images.FirstOrDefault(x => x.Id == id);
            var blImage = _mapper.Map<BLImage>(image);
            return blImage;
        }

        public IEnumerable<BLImage> GetImages()
        {
            var images = _context.Images.ToList();
            var blImages = _mapper.Map<IEnumerable<BLImage>>(images);

            return blImages;
        }

        public BLImage SaveImage(BLImage blImage)
        {
            var image = new Models.Image
            {
                Content = blImage.Content,
                PictureName = blImage.PictureName,
                PicturePath = blImage.PicturePath
            };
            _context.Images.Add(image);
            _context.SaveChanges();
            blImage.Id = image.Id;
            return blImage;
        }
        public BLImage UpdateImage(int id, BLImage blImage)
        {
            throw new NotImplementedException();
        }
    }
}
