using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AbsoluteCinema.Application.Features.Halls.Commands.CreateHallCommandHandler;

namespace AbsoluteCinema.Application.Features.Halls.Commands
{
    public sealed class CreateHallCommandHandler
        : IRequestHandler<CreateHallCommand, CreateHallResponse>
    {
        private readonly IHallRepository _hallRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateHallCommandHandler(IHallRepository hallRepository, IUnitOfWork unitOfWork)
        {
            _hallRepository = hallRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CreateHallResponse> Handle(CreateHallCommand request, CancellationToken ct)
        {
            var hall = Hall.Create(request.Name, 1, 1);//change Create method

            await _hallRepository.AddAsync(hall, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return new CreateHallResponse(hall.Id.Id,hall.HallName,
                new List<SeatDto>()
                );
        }
        public record CreateHallRequest(
            string Name
            );
        public sealed record CreateHallCommand(
            string Name
            ) : IRequest<CreateHallResponse>;
        public sealed record CreateHallResponse(
            Guid Id,
            string Name,
            List<SeatDto> Seats
        );
    }
}
