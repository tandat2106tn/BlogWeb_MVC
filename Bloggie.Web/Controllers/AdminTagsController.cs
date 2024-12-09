using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;


        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)

        {
            ValidateAddTagRequest(addTagRequest);
            if (!ModelState.IsValid)
            {
                return View();
            }
            //Mapping AddTagRequest to Tag domain model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            await tagRepository.AddAsync(tag);
            return RedirectToAction("Show");
        }


        [HttpGet]
        [ActionName("Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            //var tag =bloggieDbContext.Tags.Find(id);
            var tag = await tagRepository.GetAsync(id);

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName

                };

                return View(editTagRequest);

            }

            else
            {
                return View();

            }
        }

        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

            var updatedTag = await tagRepository.UpdateAsync(tag);
            if (updatedTag != null)
            {
                //tra ve thong bao thanh cong

                return RedirectToAction("Show");
            }
            else
            {
                //tra ve thong bao khong thanh cong
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
            }





        }

        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var deletedTag = await tagRepository.DeleteAsync(editTagRequest.Id);
            if (deletedTag != null)
            {
                //thong bao thanh cong
                return RedirectToAction("Show");
            }

            //thong bao khong thanh cong
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        private void ValidateAddTagRequest(AddTagRequest request)
        {
            if (request != null && request.DisplayName != null)

            {
                if (request.Name == request.DisplayName)
                {
                    ModelState.AddModelError("DisplayName", "DisplayName và name phải khác nhau");
                }

            }
        }

        [HttpGet]
        [ActionName("Show")]

        public async Task<IActionResult> Show(string sortOrder, string currentFilter, string searchString, int? pageNumber, int pageSize = 4)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["IdSortParm"] = sortOrder == "id_desc" ? "id" : "id_desc";
            ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["DisplayNameSortParm"] = sortOrder == "displayname" ? "displayname_desc" : "displayName";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var tags = await tagRepository.GetAllAsync(sortOrder, currentFilter, searchString, pageNumber, pageSize);
            return View(tags);
        }

    }
}
