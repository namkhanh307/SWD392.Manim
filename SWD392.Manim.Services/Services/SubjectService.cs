using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SWD392.Manim.Repositories;
using SWD392.Manim.Repositories.Entity;
using SWD392.Manim.Repositories.Repository.Interface;
using SWD392.Manim.Repositories.ViewModel.ChapterVM;
using SWD392.Manim.Repositories.ViewModel.SubjectVM;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SWD392.Manim.Services.Services
{
    public class SubjectService : ISubjectService
    {
        private const string FirebaseStorageBaseUrl = "https://firebasestorage.googleapis.com/v0/b/physic-manim.appspot.com/o";
        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork;
        public SubjectService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<GetSubjectsVM>?> GetSubjects(int index, int pageSize, string? id, string? nameSearch)
        {
            IQueryable<Subject> query = _unitOfWork.GetRepository<Subject>().Entities.Where(s => !s.DeletedAt.HasValue);

            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(lp => lp.Id.ToString().Contains(id));
            }

            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                query = query.Where(lp => lp.Name.Contains(nameSearch));
            }

            var resultQuery = await _unitOfWork.GetRepository<Subject>().GetPagging(query, index, pageSize);

            var responseItems = resultQuery.Items.Select(item =>
            {
                IEnumerable<GetChapterNamesVM> chapters = item.Chapters
                    .Select(ch => new GetChapterNamesVM { Id = ch.Id, Name = ch.Name })
                    .ToList();
                var result = _mapper.Map<GetSubjectsVM>(item);
                result.Chapters = chapters;
                return result;
            }).ToList();

            // Create paginated response
            var responsePaginatedList = new PaginatedList<GetSubjectsVM>(
                responseItems,
                resultQuery.TotalCount,
                resultQuery.PageNumber,
                resultQuery.TotalPages
            );
            return responsePaginatedList;
        }
        public async Task<GetSubjectsVM?> GetSubjectById(string id)
        {
            Subject? existedParam = await _unitOfWork.GetRepository<Subject>().Entities.Where(s => s.Id == id && !s.DeletedAt.HasValue).FirstOrDefaultAsync() ?? throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Môn học không tồn tại!");
            return _mapper.Map<GetSubjectsVM?>(existedParam);
        }

        public async Task PostSubject(PostSubjectVM model)
        {
            Subject? existedSubject = await _unitOfWork.GetRepository<Subject>().Entities.Where(s => s.Name == model.Name).FirstOrDefaultAsync();
            if (existedSubject != null)
            {
                throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Tên môn học đã tồn tại!");
            }
            var imageUrls = await UploadFileToFirebase(model.ImageLink);
            Subject subject = new Subject()
            {
                Name = model.Name,
                Image = imageUrls,
                CreatedAt = DateTime.Now,
            };
 
            await _unitOfWork.GetRepository<Subject>().InsertAsync(subject);
            await _unitOfWork.SaveAsync();
        }

        public async Task PutSubject(string id, PostSubjectVM model)
        {
            Subject? existedSubject = await _unitOfWork.GetRepository<Subject>().Entities.Where(s => s.Id == id && !s.DeletedAt.HasValue).FirstOrDefaultAsync() ?? throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Môn học không tồn tại!");
            if (existedSubject.Name != model.Name)
            {
                Subject? subjectWithSameName = await _unitOfWork.GetRepository<Subject>().Entities.Where(s => s.Name == model.Name).FirstOrDefaultAsync();


                if (subjectWithSameName != null)
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Tên môn học đã tồn tại!");
                }
            }
            _mapper.Map(model, existedSubject);
            existedSubject.UpdatedAt = DateTime.Now;
            await _unitOfWork.GetRepository<Subject>().UpdateAsync(existedSubject);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteSubject(string id)
        {
            Subject? existedSubject = await _unitOfWork.GetRepository<Subject>().GetByIdAsync(id) ?? throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Môn học không tồn tại!");
            List<Chapter> chapters = await _unitOfWork.GetRepository<Chapter>().Entities.Where(s => s.SubjectId == id && !s.DeletedAt.HasValue).ToListAsync();
            foreach (Chapter chapter in chapters)
            {
                chapter.DeletedAt = DateTime.Now;
                await _unitOfWork.GetRepository<Chapter>().UpdateAsync(chapter);
                List<Topic> topics = await _unitOfWork.GetRepository<Topic>().Entities.Where(s => s.ChapterId == chapter.Id && !s.DeletedAt.HasValue).ToListAsync();
                foreach (Topic topic in topics)
                {
                    topic.DeletedAt = DateTime.Now;
                    await _unitOfWork.GetRepository<Topic>().UpdateAsync(topic);
                    List<Problem> problems = await _unitOfWork.GetRepository<Problem>().Entities.Where(s => s.TopicId == topic.Id && !s.DeletedAt.HasValue).ToListAsync();
                    foreach (Problem problem in problems)
                    {
                        problem.DeletedAt = DateTime.Now;
                        await _unitOfWork.GetRepository<Problem>().UpdateAsync(problem);
                        List<ProblemParameter> parameters = await _unitOfWork.GetRepository<ProblemParameter>().Entities.Where(s => s.ProblemId == problem.Id && !s.DeletedAt.HasValue).ToListAsync();
                        foreach (ProblemParameter parameter in parameters)
                        {
                            parameter.DeletedAt = DateTime.Now;
                            await _unitOfWork.GetRepository<Problem>().DeleteAsync(parameter);
                        }
                    }
                }
            }
            existedSubject.DeletedAt = DateTime.Now;
            await _unitOfWork.GetRepository<Subject>().UpdateAsync(existedSubject);
            await _unitOfWork.SaveAsync();
        }
        private string ParseDownloadUrl(string responseBody, string fileName)
        {
            // This assumes the response contains a JSON object with the field "name" which is the path to the uploaded file.
            var json = JsonDocument.Parse(responseBody);
            var nameElement = json.RootElement.GetProperty("name");
            var downloadUrl = $"{FirebaseStorageBaseUrl}/{Uri.EscapeDataString(nameElement.GetString())}?alt=media";
            return downloadUrl;
        }
        /*private async Task<List<string>> UploadFilesToFirebase(List<IFormFile> formFiles)
        {
            var uploadedUrls = new List<string>();

            try
            {
                using (var client = new HttpClient())
                {
                    foreach (var formFile in formFiles)
                    {
                        if (formFile.Length > 0)
                        {
                            string fileName = Path.GetFileName(formFile.FileName);
                            string firebaseStorageUrl = $"{FirebaseStorageBaseUrl}?uploadType=media&name=images/{Guid.NewGuid()}_{fileName}";

                            using (var stream = new MemoryStream())
                            {
                                await formFile.CopyToAsync(stream);
                                stream.Position = 0;
                                var content = new ByteArrayContent(stream.ToArray());
                                content.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);

                                var response = await client.PostAsync(firebaseStorageUrl, content);
                                if (response.IsSuccessStatusCode)
                                {
                                    var responseBody = await response.Content.ReadAsStringAsync();
                                    var downloadUrl = ParseDownloadUrl(responseBody, fileName);
                                    uploadedUrls.Add(downloadUrl);
                                }
                                else
                                {
                                    var errorMessage = $"Error uploading file {fileName} to Firebase Storage. Status Code: {response.StatusCode}\nContent: {await response.Content.ReadAsStringAsync()}";

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return uploadedUrls;
        }*/
        private async Task<string> UploadFileToFirebase(IFormFile formFile)
        {
            string uploadedUrl = null;

            try
            {
                using (var client = new HttpClient())
                {
                    if (formFile.Length > 0)
                    {
                        string fileName = Path.GetFileName(formFile.FileName);
                        string firebaseStorageUrl = $"{FirebaseStorageBaseUrl}?uploadType=media&name=images/{Guid.NewGuid()}_{fileName}";

                        using (var stream = new MemoryStream())
                        {
                            await formFile.CopyToAsync(stream);
                            stream.Position = 0;
                            var content = new ByteArrayContent(stream.ToArray());
                            content.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);

                            var response = await client.PostAsync(firebaseStorageUrl, content);
                            if (response.IsSuccessStatusCode)
                            {
                                var responseBody = await response.Content.ReadAsStringAsync();
                                uploadedUrl = ParseDownloadUrl(responseBody, fileName);
                            }
                            else
                            {
                                var errorMessage = $"Lỗi khi tải lên tệp {fileName} lên Firebase Storage. Mã trạng thái: {response.StatusCode}\nNội dung: {await response.Content.ReadAsStringAsync()}";

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return uploadedUrl;
        } 
    }



    
}
