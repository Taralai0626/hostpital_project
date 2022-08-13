using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HTTP5212_HospitalProject_Team1.Models;
using System.Diagnostics;

namespace HTTP5212_HospitalProject_Team1.Controllers
{
    public class RoomDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/RoomData/ListRooms
        [HttpGet]
        [ResponseType(typeof(RoomDto))]
        public IEnumerable<RoomDto> ListRooms()
        {
            List<Room> Rooms = db.Rooms.ToList();
            List<RoomDto> RoomDtos = new List<RoomDto>();

            Rooms.ForEach(r => RoomDtos.Add(new RoomDto()
            {
                RoomId = r.RoomId,
                RoomType = r.RoomType,
                RoomNumber = r.RoomNumber,
                Availability = r.Availability,
                PatientID = r.Patient.PatientID,
                FirstName = r.Patient.FirstName,
                LastName = r.Patient.LastName,
            }));

            return RoomDtos;
        }

        // GET: api/RoomData/FindRooms/5
        [ResponseType(typeof(Room))]
        [HttpGet]
        public IHttpActionResult FindRoom(int id)
        {
            Room Room = db.Rooms.Find(id);
            RoomDto RoomDto = new RoomDto()
            {
                RoomId = Room.RoomId,
                RoomType = Room.RoomType,
                RoomNumber = Room.RoomNumber,
                Availability = Room.Availability,
                PatientID = Room.Patient.PatientID,
                FirstName = Room.Patient.FirstName,
                LastName = Room.Patient.LastName,
            };
            if (Room == null)
            {
                return NotFound();
            }

            return Ok(RoomDto);
        }

        // POST: api/RoomData/UpdateRoom/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateRoom(int id, Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != room.RoomId)
            {
                return BadRequest();
            }

            db.Entry(room).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Receives room picture data, uploads it to the webserver and updates the room's HasPic option
        /// </summary>
        /// <param name="id">the room id</param>
        /// <returns>status code 200 if successful.</returns>
        /// <example>
        /// curl -F room=@file.jpg "https://localhost:xx/api/roomdata/uploadroompic/2"
        /// POST: api/roomData/UpdateroomPic/3
        /// HEADER: enctype=multipart/form-data
        /// FORM-DATA: image
        /// </example>
        /// https://stackoverflow.com/questions/28369529/how-to-set-up-a-web-api-controller-for-multipart-form-data

        [HttpPost]
        public IHttpActionResult UploadRoomPic(int id)
        {

            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var roomPic = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (roomPic.ContentLength > 0)
                    {
                        //establish valid file types (can be changed to other file extensions if desired!)
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(roomPic.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/rooms/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Rooms/"), fn);

                                //save the file
                                roomPic.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the room haspic and picextension fields in the database
                                Room Selectedroom = db.Rooms.Find(id);
                                Selectedroom.RoomHasPic = haspic;
                                Selectedroom.PicExtension = extension;
                                db.Entry(Selectedroom).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("room Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                                return BadRequest();
                            }
                        }
                    }

                }

                return Ok();
            }
            else
            {
                //not multipart form data
                return BadRequest();

            }

        }
        // POST: api/RoomData/AddRoom
        [ResponseType(typeof(Room))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddRoom(Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rooms.Add(room);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = room.RoomId }, room);
        }

        // POST: api/RoomData/DeleteRoom/5
        [ResponseType(typeof(Room))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteRoom(int id)
        {
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            db.Rooms.Remove(room);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoomExists(int id)
        {
            return db.Rooms.Count(e => e.RoomId == id) > 0;
        }
    }
}