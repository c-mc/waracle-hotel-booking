using HotelBookings.Common.DTOs;
using HotelBookings.Domain.Entities;
using HotelBookings.Domain.Enums;
using HotelBookings.Domain.Interfaces;
using HotelBookings.Domain.Specifications;
using System.Net;

namespace HotelBookings.Application.Services
{
    public class AdminService(IGenericRepository<Hotel> hotelRepository,
                              IGenericRepository<Room> roomRepository,
                              IBookingRepository bookingRepository)
                 : IAdminService
    {
        public async Task<BaseResultDto> SeedDatabaseAsync()
        {
            if (! await IsDatabaseEmpty())
            {
                return new BaseResultDto { StatusCode = HttpStatusCode.Conflict, Message = "The database contains existing data. Reset the database before attempting to seed it." };
            }

            var rowsAdded = await CreateHotels();

            return rowsAdded > 0 ? 
                new BaseResultDto { StatusCode = HttpStatusCode.Created, Message = "Database seeded successfully!" }
                :
                new BaseResultDto { StatusCode = HttpStatusCode.Conflict, Message = "Failed to seed database." };
        }

        public BaseResultDto RemoveDatabaseDataAsync()
        {
            try
            {
                bookingRepository.DeleteAll();
                hotelRepository.DeleteAll(); // this will remove rooms too via a cascade
            }
            catch (Exception)
            {
                return new BaseResultDto { StatusCode = HttpStatusCode.Conflict, Message = "Failed to remove all data from the database."};
            }
            return new BaseResultDto { StatusCode = HttpStatusCode.OK, Message = "All data removed from database." };
        }

        private async Task<bool> IsDatabaseEmpty()
        {
            // Check if there is any data in the database - i.e. at least a single record in any table
            
            BaseSpecification<Booking> bookingSpec = new(b => true);
            bookingSpec.ApplyPagging(1, 0);
            var booking = await bookingRepository.GetWithSpecAsync(bookingSpec);

            if (booking?.Id != null)
            {
                return false;
            }

            BaseSpecification<Room> roomSpec = new(b => true);
            roomSpec.ApplyPagging(1, 0);
            var room = await roomRepository.GetWithSpecAsync(roomSpec);

            if (room?.Id != null)
            {
                return false;
            }

            BaseSpecification<Hotel> hotelSpec = new(b => true);
            hotelSpec.ApplyPagging(1, 0);
            var hotel = await hotelRepository.GetWithSpecAsync(hotelSpec);

            if (hotel?.Id != null)
            {
                return false;
            }

            return true;
        }

        private static Room CreateRoom(RoomType roomType, Hotel hotel)
        {
            Random rnd = new();
            var capacity = roomType.Equals(RoomType.Deluxe) ? rnd.Next(2, 6) : (int)roomType+1;
            return new Room { Id = Guid.NewGuid(), HotelId = hotel.Id, Capacity = capacity, Type = roomType, Hotel = hotel };
        }

        private static void CreateRooms(ref Hotel hotel)
        {
            Random rnd = new();
            for (int i = 0; i < 6; i++)
            {
                hotel.AddRoom(CreateRoom((RoomType)rnd.Next(0, 3), hotel));
            }
        }

        private async Task<int> CreateHotels()
        {
            var rowsAdded = 0;

            // TODO: Could have this as a JSON file
            var hotels = new[] {
                new
                {
                    Name =  "The Ritz Paris",
                    Description = "An iconic luxury hotel in Paris known for its opulent suites and historic clientele, including Coco Chanel and Ernest Hemingway.",
                    Address = "15 Place Vendôme, 75001 Paris, France"
                },
                new
                {
                    Name = "The Plaza Hotel",
                    Description = "A New York City landmark offering timeless elegance and grandeur overlooking Central Park.",
                    Address = "768 5th Ave, New York, NY 10019, USA"
                },
                new
                {
                    Name = "Marina Bay Sands",
                    Description = "Singapore’s futuristic hotel famous for its rooftop infinity pool and panoramic city views.",
                    Address = "10 Bayfront Avenue, Singapore 018956"
                },
                new
                {
                    Name = "Burj Al Arab Jumeirah",
                    Description = "A sail-shaped hotel in Dubai, often cited as the world's most luxurious hotel.",
                    Address = "Jumeirah Street, Dubai, United Arab Emirates"
                },
                new
                {
                    Name = "The Savoy",
                    Description = "Historic London hotel offering old-world charm and riverside luxury along the Thames.",
                    Address = "Strand, London WC2R 0EZ, United Kingdom"
                },
                new
                {
                    Name = "Hotel de Glace",
                    Description = "A unique ice hotel rebuilt every winter near Quebec City, featuring ice sculptures and frozen suites.",
                    Address = "1860 Boulevard Valcartier, Saint-Gabriel-de-Valcartier, QC G0A 4S0, Canada"
                },
                new
                {
                    Name = "Taj Mahal Palace",
                    Description = "A historic 5-star hotel in Mumbai overlooking the Gateway of India, blending Indo-Saracenic architecture with modern luxury.",
                    Address = "Apollo Bandar, Colaba, Mumbai, Maharashtra 400001, India"
                },
                new
                {
                    Name = "The Peninsula Hong Kong",
                    Description = "One of Asia’s grandest hotels, known for its colonial charm and Rolls-Royce fleet.",
                    Address = "Salisbury Road, Kowloon, Hong Kong"
                },
                new
                {
                    Name = "Raffles Singapore",
                    Description = "A colonial-era hotel that introduced the famous Singapore Sling cocktail and continues to epitomize elegance.",
                    Address = "1 Beach Road, Singapore 189673"
                },
                new
                {
                    Name = "Hotel Danieli",
                    Description = "A Venetian Gothic palace turned luxury hotel, offering views of the Grand Canal and St. Mark’s Square.",
                    Address = "Riva degli Schiavoni, 4196, 30122 Venezia VE, Italy"
                },
                new
                {
                    Name = "Four Seasons Resort Bora Bora",
                    Description = "A tropical paradise offering overwater bungalows and crystal-clear lagoon views.",
                    Address = "Motu Tehotu, Bora Bora, French Polynesia"
                },
                new
                {
                    Name = "The Beverly Hills Hotel",
                    Description = "A Hollywood icon known as 'The Pink Palace', famous for its celebrity guests and palm-lined pool.",
                    Address = "9641 Sunset Blvd, Beverly Hills, CA 90210, USA"
                },
                new
                {
                    Name = "Emirates Palace Mandarin Oriental",
                    Description = "An extravagant hotel in Abu Dhabi, showcasing Arabian opulence and world-class hospitality.",
                    Address = "West Corniche Road, Abu Dhabi, United Arab Emirates"
                },
                new
                {
                    Name = "Hotel Cipriani",
                    Description = "An exclusive Venetian retreat known for its lagoon views and impeccable service.",
                    Address = "Giudecca 10, 30133 Venezia VE, Italy"
                },
                new
                {
                    Name = "Claridge’s",
                    Description = "A Mayfair institution renowned for art deco interiors and high society gatherings.",
                    Address = "Brook Street, Mayfair, London W1K 4HR, United Kingdom"
                },
                new
                {
                    Name = "The Waldorf Astoria New York",
                    Description = "A legendary New York hotel with a storied past and elegant Art Deco design.",
                    Address = "301 Park Ave, New York, NY 10022, USA"
                },
                new
                {
                    Name = "Fairmont Le Château Frontenac",
                    Description = "A castle-like hotel towering over Old Quebec, one of the most photographed hotels in the world.",
                    Address = "1 Rue des Carrières, Québec, QC G1R 4P5, Canada"
                },
                new
                {
                    Name = "Aman Tokyo",
                    Description = "A serene urban sanctuary blending minimalist design with panoramic city views.",
                    Address = "The Otemachi Tower, 1-5-6 Otemachi, Chiyoda City, Tokyo 100-0004, Japan"
                },
                new
                {
                    Name = "The Dorchester",
                    Description = "A London institution offering refined luxury and views of Hyde Park.",
                    Address = "53 Park Lane, London W1K 1QA, United Kingdom"
                },
                new
                {
                    Name = "The Atlantis Paradise Island",
                    Description = "A vast ocean-themed resort featuring marine habitats, water parks, and luxury accommodations.",
                    Address = "One Casino Drive, Paradise Island, Bahamas"
                }
            };

            foreach (var hotel in hotels)
            {
                var newHotel = new Hotel { Id = Guid.NewGuid(), Name = hotel.Name, Description = hotel.Description, Address = hotel.Address };

                CreateRooms(ref newHotel);

                rowsAdded += await hotelRepository.AddAsync(newHotel);
            }

            return rowsAdded;
        }
    }
}
