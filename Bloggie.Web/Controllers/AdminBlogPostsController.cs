using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminBlogPostsController : Controller
	{
		private readonly ITagRepository tagRepository;
		private readonly IBlogPostRepository blogPostRepository;

		public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
		{
			this.tagRepository = tagRepository;
			this.blogPostRepository = blogPostRepository;
		}
		[HttpGet]
		public async Task<IActionResult> Add()
		{
			//lấy danh sách tag
			var tags = await tagRepository.GetAllAsync();
			var model = new AddBlogPostRequest
			{
				Tags = tags.Select(x => new SelectListItem
				{
					Text = x.DisplayName,
					Value = x.Id.ToString()
				})
			};
			return View(model);
		}
		[HttpPost]
		[ActionName("Add")]
		public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
		{
			//map view to domain
			var blogPost = new BlogPost
			{
				Heading = addBlogPostRequest.Heading,
				PageTitle = addBlogPostRequest.PageTitle,
				Content = addBlogPostRequest.Content,
				ShortDescription = addBlogPostRequest.ShortDescription,
				FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
				UrlHandle = addBlogPostRequest.UrlHandle,
				PublishedDate = addBlogPostRequest.PublishedDate,
				Author = addBlogPostRequest.Author,
				Visible = addBlogPostRequest.Visible,

			};
			//map tags form selectedtags
			var selectedTags = new List<Tag>();
			foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
			{
				var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
				var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);
				if (existingTag != null)
				{
					selectedTags.Add(existingTag);
				}
			}
			//Mapping tags back to domain model

			blogPost.Tags = selectedTags;
			await blogPostRepository.AddAsync(blogPost);

			return RedirectToAction("Show");
		}

		[HttpPost]
		[ActionName("PostPreview")]
		public async Task<IActionResult> PostPreView(AddBlogPostRequest addBlogPostRequest)
		{
			var blogPost = new BlogPost
			{
				Heading = addBlogPostRequest.Heading,
				PageTitle = addBlogPostRequest.PageTitle,
				Content = addBlogPostRequest.Content,
				ShortDescription = addBlogPostRequest.ShortDescription,
				FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
				UrlHandle = addBlogPostRequest.UrlHandle,
				PublishedDate = addBlogPostRequest.PublishedDate,
				Author = addBlogPostRequest.Author,
				Visible = addBlogPostRequest.Visible,

			};
			//map tags form selectedtags
			var selectedTags = new List<Tag>();
			foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
			{
				var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
				var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);
				if (existingTag != null)
				{
					selectedTags.Add(existingTag);
				}
			}
			//Mapping tags back to domain model

			blogPost.Tags = selectedTags;

			return View(blogPost);
		}

		[HttpGet]
		[ActionName("Show")]

		public async Task<IActionResult> Show(string sortOrder, string currentFilter, string searchString, int? pageNumber, int pageSize = 4)
		{
			ViewData["CurrentSort"] = sortOrder;
			ViewData["IdSortParm"] = sortOrder == "id_desc" ? "id" : "id_desc";
			ViewData["HeadingSortParm"] = sortOrder == "heading" ? "heading_desc" : "heading";
			ViewData["PublishDateSortParm"] = sortOrder == "publish_date" ? "publish_date_desc" : "publish_date";

			if (searchString != null)
			{
				pageNumber = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewData["CurrentFilter"] = searchString;

			var blogPosts = await blogPostRepository.GetAllAsync(sortOrder, currentFilter, searchString, pageNumber, pageSize);
			return View(blogPosts);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			//tra lai blog dang try van
			var blogPort = await blogPostRepository.GetAsync(id);
			var tagDomainModel = await tagRepository.GetAllAsync();
			if (blogPort != null)
			{
				var model = new EditBlogPostRequest
				{
					Id = blogPort.Id,
					Heading = blogPort.Heading,
					PageTitle = blogPort.PageTitle,
					Content = blogPort.Content,
					Author = blogPort.Author,
					FeaturedImageUrl = blogPort.FeaturedImageUrl,
					UrlHandle = blogPort.UrlHandle,
					ShortDescription = blogPort.ShortDescription,
					PublishedDate = blogPort.PublishedDate,
					Visible = blogPort.Visible,
					Tags = tagDomainModel.Select(x => new SelectListItem
					{
						Text = x.Name,
						Value = x.Id.ToString()
					}),
					SelectedTags = blogPort.Tags.Select(x => x.Id.ToString()).ToArray()
				};

				return View(model);
			}


			//truyen data cho view
			return View(null);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
		{
			//map view model back to domain model
			var blogPostDomainModel = new BlogPost
			{
				Id = editBlogPostRequest.Id,
				Heading = editBlogPostRequest.Heading,
				PageTitle = editBlogPostRequest.PageTitle,
				Content = editBlogPostRequest.Content,
				Author = editBlogPostRequest.Author,
				ShortDescription = editBlogPostRequest.ShortDescription,
				FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
				PublishedDate = editBlogPostRequest.PublishedDate,
				UrlHandle = editBlogPostRequest.UrlHandle,
				Visible = editBlogPostRequest.Visible

			};
			//map tag to model
			var selectedTags = new List<Tag>();
			foreach (var selectedTag in editBlogPostRequest.SelectedTags)
			{
				if (Guid.TryParse(selectedTag, out var tag))
				{
					var foundTag = await tagRepository.GetAsync(tag);
					if (foundTag != null)
					{
						selectedTags.Add(foundTag);
					}
				}
			}
			blogPostDomainModel.Tags = selectedTags;


			//submit to update
			var updateBlog = await blogPostRepository.UpdateAsync(blogPostDomainModel);
			if (updateBlog != null)
			{
				return RedirectToAction("Show");
			}
			return RedirectToAction("Edit");

			//tro ve show
		}
		[HttpPost]
		public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
		{
			//send to repository
			var deletedBlogPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);
			if (deletedBlogPost != null)
			{
				//tra ve thong bao thanh cong
				return RedirectToAction("Show");
			}
			return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });

			//display
		}

		[HttpGet]
		public async Task<IActionResult> Preview(Guid id)
		{
			var blogPost = await blogPostRepository.GetAsync(id);
			if (blogPost == null)
			{
				return NotFound();
			}
			return View(blogPost);
		}

	}
}
