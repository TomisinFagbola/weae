using Application.DataTransferObjects;
using Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IStateService
    {
        Task<SuccessResponse<StateDto>> RegisterState(StateCreateDto model, Guid id);

        Task<SuccessResponse<StateDto>> GetStateById(Guid countryId, Guid id);

        Task<PagedResponse<IEnumerable<StateDto>>> GetStates(Guid countryId, StateParameter parameters, string actionName, IUrlHelper urlHelper);

        Task<SuccessResponse<StateDto>> UpdateState(StateUpdateDto model, Guid countryId, Guid id);

        Task RemoveState(Guid countryId, Guid id);


    }
}
