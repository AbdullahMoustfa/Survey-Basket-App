using SurveyApp.Api.Contracts.Questions;

namespace SurveyApp.Api.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly ApplicationDbContext _dbContext;

        public QuestionService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken = default)
        {
            var pollExists = await _dbContext.Polls.AnyAsync(x => x.Id == pollId, cancellationToken);

            if (!pollExists)
                return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);

            var questions = await _dbContext.Questions
                .Where(x => x.PollId == pollId) // Fix: Use PollId instead of Id
                .Include(a => a.Answers)
                .ProjectToType<QuestionResponse>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<QuestionResponse>>(questions);    
        }
        public async Task<Result<QuestionResponse>> GetByIdAsync(int pollId, int id, CancellationToken cancellationToken)
        {
            var pollExists = await _dbContext.Polls.AnyAsync(x => x.Id == pollId);
                
                if(!pollExists)
                return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);   

            var questions = await _dbContext.Questions
                .Where(x=>x.PollId == pollId && x.Id==id)
                .Include(a => a.Answers)
                .ProjectToType<QuestionResponse>()
                .AsNoTracking() 
                .SingleOrDefaultAsync(cancellationToken);   

            if(questions is null)
                return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);   

            return Result.Success(questions); 
        }
        public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken)
        {
            // Check if the poll exists
            var pollExists = await _dbContext.Polls.AnyAsync(x => x.Id == pollId, cancellationToken);
        
            if (!pollExists)
                return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);
        
            // Check if the question content is duplicated
            var questionExists = await _dbContext.Questions.AnyAsync(x => x.Content ==request.Content, cancellationToken);
        
            if(questionExists)
                return Result.Failure<QuestionResponse>(QuestionErrors.DuplicatedQuestionContent);
        
            // Map the request to a Question entity
            var question = request.Adapt<Question>();
            question.PollId = pollId;
        
            // Add answers to the question
            request.Answers.ForEach(answer => question.Answers.Add(new Answer { Content = answer }));
        
            // Save the question to the database
            await _dbContext.AddAsync(question);    
            await _dbContext.SaveChangesAsync();    
        
            return Result.Success(question.Adapt<QuestionResponse>());
        }
        public async Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request, CancellationToken cancellationToken)
        {
            var questionIsExist = await _dbContext.Questions
                .AnyAsync(
                    x => x.PollId==pollId
                    && x.Id != id
                    && x.Content == request.Content 
                    ,cancellationToken
                );

            if (!questionIsExist)
                return Result.Failure(QuestionErrors.QuestionNotFound);

            var question = await _dbContext.Questions
                .Include(x=>x.Answers)
                .SingleOrDefaultAsync(x => x.PollId==pollId&&x.Id==id, cancellationToken);

           if(question is null)
                return Result.Failure(QuestionErrors.QuestionNotFound);

            question.Content = request.Content; 

            //current answers
            var currentAnswers = question.Answers.Select(x=>x.Content).ToList();    

            //new Answers
            var newAnswers = request.Answers.Except(currentAnswers).ToList();   

            foreach (var answer in newAnswers) 
                question.Answers.Add(new Answer { Content = answer });

            // Add new answers
            foreach (var answer in newAnswers)
            {
                question.Answers.Add(new Answer { Content = answer });
            }

            // Update active status for all answers
            foreach (var answer in question.Answers)
            {
                answer.IsActive = request.Answers.Contains(answer.Content);
            }


            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();    
        }

        public async Task<Result> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken)
        {
            var question = await _dbContext.Questions.SingleOrDefaultAsync(x => x.PollId == pollId && x.Id == id, cancellationToken);

            if(question is null)
                return Result.Failure(QuestionErrors.QuestionNotFound); 

            question.IsActive = !question.IsActive; 
            await _dbContext.SaveChangesAsync();    

            return Result.Success();    
        }

    }
}
