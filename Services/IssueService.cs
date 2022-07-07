using Marc2.Contracts.Issue;
using Marc2.Domain.Entities;
using Marc2.Domain.Exceptions;
using Marc2.Domain.Repositories;
using Marc2.Services.Abstractions;

namespace Marc2.Services
{
    public class IssueService : IIssueService
    {
        private readonly IRepositoryManager _repository;
        public IssueService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<IssueDto> CreateIssueAsync(CreateIssueDto createIssueDto, string userEmail)
        {
            var accident = await _repository.AccidentRepository.GetAccidentById(createIssueDto.AccidentId);
            var user = await _repository.UserRepository.GetByEmailAsync(userEmail);

            if (accident == null)
                throw new AccidentNotFoundException(createIssueDto.AccidentId);

            if (accident.OrganizationId != user.Organization.Id)
                throw new InvalidAccidentOrganizationException();

            var issue = MapFromCreateIssueDto(createIssueDto);
            _repository.IssueRepository.CreateIssue(issue);
            await _repository.UnitOfWork.SaveChangesAsync();

            await SetIssueAssignments(createIssueDto.WorkShiftIds, issue);
            _repository.IssueRepository.UpdateIssue(issue);
            await _repository.UnitOfWork.SaveChangesAsync();

            return MapFromIssueToDto(issue);
        }

        public async Task UpdateIssueAsync(UpdateIssueDto updateIssueDto, string userEmail, int issueId)
        {
            var issue = await _repository.IssueRepository.GetIssueByIdAsync(issueId);
            var user = await _repository.UserRepository.GetByEmailAsync(userEmail);

            if (issue == null)
                throw new IssueNotFoundException(issueId);

            if (issue.Accident.OrganizationId != user.Organization.Id)
                throw new InvalidAccidentOrganizationException();

            MapFromUpdateIssueDto(updateIssueDto, issue);

            foreach (var assignment in issue.Assignments)
                _repository.AssignmentRepository.DeleteAssignment(assignment);
            await _repository.UnitOfWork.SaveChangesAsync();

            await SetIssueAssignments(updateIssueDto.WorkShiftIds, issue);
            _repository.IssueRepository.UpdateIssue(issue);
            await _repository.UnitOfWork.SaveChangesAsync();
        }

        private void MapFromUpdateIssueDto(UpdateIssueDto updateIssueDto, Issue issue)
        {
            issue.Text = updateIssueDto.Text;
            issue.IsCompleted = updateIssueDto.IsCompleted;
            issue.Lattitude = updateIssueDto.Lattitude;
            issue.Longtitude = updateIssueDto.Longtitude;
        }

        private Issue MapFromCreateIssueDto(CreateIssueDto createIssueDto)
        {
            var issue = new Issue
            {
                AccidentId = createIssueDto.AccidentId,
                Lattitude = createIssueDto.Lattitude,
                Longtitude = createIssueDto.Longtitude,
                Text = createIssueDto.Text,
                CreatedAt = DateTime.Now
            };

            return issue;
        }

        private IssueDto MapFromIssueToDto(Issue issue)
        {
            var issueDto = new IssueDto
            {
                Lattitude = issue.Lattitude == null ? 0 : (double) issue.Lattitude,
                Longtitude = issue.Longtitude == null ? 0 :(double) issue.Longtitude,
                WorkShiftIds = issue.Assignments.Select(a => a.WorkShiftId).ToList(),
                AccidentId = issue.AccidentId,
                IsCompleted = issue.IsCompleted,
                CreatedAt = issue.CreatedAt,
                IssueId = issue.Id,
                Text = issue.Text,
            };
            return issueDto;
        }

        private async Task SetIssueAssignments(IEnumerable<int> workShiftIds, Issue issue)
        {
            issue.Assignments = new List<Assignment>();
            foreach (var workShiftId in workShiftIds)
            {
                var workShift = await _repository.WorkShiftRepository.GetWorkShiftByIdAsync(workShiftId);
                if (workShift == null)
                    throw new WorkShiftNotFoundException(workShiftId);

                if (await _repository.WorkShiftRepository.IsWorkShiftBelongedToAnotherAccident(issue.AccidentId, workShiftId))
                    throw new AmbigousAccidentsIssuesException();

                issue.Assignments.Add(new Assignment { IssueId = issue.Id, WorkShiftId = workShiftId });
            }
        }

        public async Task DeleteIssueAsync(int issueId, string userEmail)
        {
            var issue = await _repository.IssueRepository.GetIssueByIdAsync(issueId);
            var user = await _repository.UserRepository.GetByEmailAsync(userEmail);

            if (issue == null)
                throw new IssueNotFoundException(issueId);

            if (issue.Accident.OrganizationId != user.Organization.Id)
                throw new InvalidAccidentOrganizationException();

            foreach (var assignment in issue.Assignments)
                _repository.AssignmentRepository.DeleteAssignment(assignment);
            await _repository.UnitOfWork.SaveChangesAsync();

            _repository.IssueRepository.DeleteIssue(issue);
            await _repository.UnitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<IssueDto>> GetAllIssuesByAccident(int accidentId, string userEmail)
        {
            var accident = await _repository.AccidentRepository.GetAccidentById(accidentId);
            var user = await _repository.UserRepository.GetByEmailAsync(userEmail);

            if (accident == null)
                throw new AccidentNotFoundException(accidentId);

            if (accident.OrganizationId != user.Organization.Id)
                throw new InvalidAccidentOrganizationException();

            var issues = await _repository.IssueRepository.GetAllIssuesByIdAccidentAsync(accidentId);
            return issues.Select(i => MapFromIssueToDto(i)).ToList();
        }

        public async Task ChangeIssueStatusAsync(int issueId, bool completenessStatus, string userEmail)
        {
            var user = await _repository.UserRepository.GetByEmailAsync(userEmail);
            var issue = await _repository.IssueRepository.GetIssueByIdAsync(issueId);

            if (issue == null)
                throw new IssueNotFoundException(issueId);

            if (issue.Accident.OrganizationId != user.Organization.Id)
                throw new InvalidAccidentOrganizationException();

            issue.IsCompleted = completenessStatus;
            _repository.IssueRepository.UpdateIssue(issue);
            await _repository.UnitOfWork.SaveChangesAsync();
        }
    }
}
