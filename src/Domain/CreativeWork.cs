using System;
using System.Collections.Generic;

using System.Linq;
using Events;
using System.Drawing;

namespace Domain
{
	public class CreativeWork : Thing
	{
		public enum WorkCollections
		{
			Archive,
			Museum
		};

		public IEnumerable<Tag> Tags { get { return base.RelatedObjects<Tag>(Relation.VerbType.TaggedAs); } }
		public IEnumerable<Category> Categories { get { return RelatedObjects<Category>(Relation.VerbType.CategorizedAs); } }
		public IEnumerable<Material> Materials { get { return RelatedObjects<Material>(Relation.VerbType.MadeOf); } }

		public string Title { get; set; }
		public string Series { get; set; }
		public string Description { get; set; }

		public DateTime DateCreated { get; set; }
		public bool IsMatureContent { get; set; }

		private List<StoredImage> _images;
		public IEnumerable<StoredImage> Images
		{
			get
			{
				if (_images == null)
					_images = new List<StoredImage>();
				return _images;
			}
		}

		public IEnumerable<StoredImage> SecondaryImages
		{
			get
			{
				return Images.Where(i => i.IsPrimary == false);
			}
		}

		public WorkCollections Location { get; set; }

		public Thing Author
		{
			get { return RelatedSubject<CreativeWork>(Relation.VerbType.Authored); }
			set
			{
				if (Author != null)
					throw new ArgumentException("There already exists an (primary) author for this creative work");
				AddSubject(Relation.VerbType.Authored, value);
			}
		}

		public Person AuthorPerson { get { return Author as Person; } }
		//		public Organization AuthorOrganization { get { return Author as Organization; } }

		internal void AddTag(Tag tag)
		{
			AddObject(Relation.VerbType.TaggedAs, tag);
		}

		internal void RemoveTag(Tag tag)
		{
			RemoveObject(Relation.VerbType.TaggedAs, tag);
		}

		internal void AddCategory(Category category)
		{
			AddObject(Relation.VerbType.CategorizedAs, category);
		}

		internal void RemoveCategory(Category category)
		{
			RemoveObject(Relation.VerbType.CategorizedAs, category);
		}

		internal void AddMaterial(Material material)
		{
			AddObject(Relation.VerbType.MadeOf, material);
		}

		internal void RemoveMaterial(Material material)
		{
			RemoveObject(Relation.VerbType.MadeOf, material);
		}

		//internal void SwitchPrimaryImage(System.Drawing.Image img)
		//{
		//    if (_images == null)
		//    {
		//        _images = new List<Image>();
		//        Image mvimg = new Image(img);
		//        mvimg.IsPrimary = true;
		//    }
		//    else
		//    {
		//        foreach (Image mvimg in _images)
		//        {
		//            if (mvimg == FindMueVueImageFromImage(img))
		//            {
		//                mvimg.IsPrimary = true;
		//            }
		//            else
		//            {
		//                mvimg.IsPrimary = false;
		//            }
		//        }
		//    }


		//}

		//private Image FindMueVueImageFromImage(System.Drawing.Image img)
		//{
		//    if (_images == null) return default(Image);
		//    else
		//    {
		//        return _images.FirstOrDefault(i => i.Image == img);
		//    }
		//}

		public override string ToString()
		{
			throw new NotImplementedException();
		}

		public override void OnReceiving(EventBase e)
		{
			throw new NotImplementedException();
			//switch(e)
			//{
			//    //case AuthorUploadedCreativeWork:
			//    //case PatronUploadedCreativeWork:
			//    //case AuthorModifiedCreativeWork:
			//    //case PatronModifiedCreativeWork:
			//    //case CreativeWorkRated:
			//    //case CreativeWorkCritqued:
			//    //case CreativeWorkRented:
			//    //case CreativeWorkSold:
			//    //default:
			//}
		}
	}
}