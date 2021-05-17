using AutoMapper;
using Library.API.Resources;
using Library.Domain.Models;

namespace Library.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookResource>().ReverseMap();
            CreateMap<Author, AuthorResource>().ReverseMap();
            
            CreateMap<SaveBookResource, Book>();
            CreateMap<SaveAuthorResource, Author>();
        }
    }
}