using Bongo.DataAccess;
using Bongo.DataAccess.Repository;
using Bongo.Models.Model;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bango.DataAccess
{
    [TestFixture]
    public class StudyRoomBookingRepositoryTests
    {
        private StudyRoomBooking studyRoomBooking_one;
        private StudyRoomBooking studyRoomBooking_two;

        public StudyRoomBookingRepositoryTests()
        {
            studyRoomBooking_one = new StudyRoomBooking()
            {
               FirstName ="Ben1",
               LastName ="spark1",
               Date= new DateTime(2023,2,2),
               Email="test@test.com",
               BookingId=11,
               StudyRoomId=1,
            };

            studyRoomBooking_two = new StudyRoomBooking()
            {
                FirstName = "Ben1",
                LastName = "spark1",
                Date = new DateTime(2023, 2, 2),
                Email = "test@test.com",
                BookingId = 22,
                StudyRoomId = 2,
            };
        }

        [Test]
        [Order(1)]
        public void SaveBooking_Booking_one_CheckTheValuesFromDatabase()
        {
            //arrange
            var option= new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "tempBango").Options;

            //act
            using(var context = new ApplicationDbContext(option))
            {
                var repository = new StudyRoomBookingRepository(context);
                repository.Book(studyRoomBooking_one);

            }

            //assert
            using (var context = new ApplicationDbContext(option))
            {
                var bookingFromdb = context.StudyRoomBookings.FirstOrDefault(u => u.BookingId == 11);

                ClassicAssert.AreEqual(studyRoomBooking_one.BookingId, bookingFromdb.BookingId);
               

            }
        }

        [Test]
        [Order(2)]

        public void GetAllBooking_Booking_oneandtwo_CheckTheValuesFromDatabase()
        {
            //arrange
            var expectedResult = new List<StudyRoomBooking> { studyRoomBooking_one, studyRoomBooking_two };
            var option = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "tempBango").Options;

            using(var context = new ApplicationDbContext(option))
            {
                context.Database.EnsureDeleted();
                var repository = new StudyRoomBookingRepository(context);
                repository.Book(studyRoomBooking_one);
                repository.Book(studyRoomBooking_two);
            }

            //act
            List<StudyRoomBooking> actualList;
            using (var context = new ApplicationDbContext(option))
            {
                var repository = new StudyRoomBookingRepository(context);
                actualList = repository.GetAll(null).ToList();

            }

            //assert
            CollectionAssert.AreEqual(expectedResult, actualList,new  BookingCompare());
        }

    }

    public class BookingCompare : IComparer
    {
        public int Compare(object? x, object? y)
        {
            var booking1 = (StudyRoomBooking)x;
            var booking2 = (StudyRoomBooking)y;

            if (booking1.BookingId != booking2.BookingId)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

}
