using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
     [Authorize]
    public class MembersController(IMemberRepository memberRepository) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetMembers()
        {
            return Ok(await memberRepository.GetMembersAsync());
        }
       
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(string id)
        {
            var member=await memberRepository.GetMemberByIdAsync(id);
            if(member!=null)
            return member;

            else return NotFound();
        }
        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>>GetMemberPhotos(string id)
        {
            return Ok(await memberRepository.GetPhotosForMemberAsync(id));
        }
        [HttpPut]
        public async Task<ActionResult> UpdateMember(MemberUpdatedDto memberUpdatedDto)
        {
            var memberId=User.GetMemberId();

            var member=await memberRepository.GetMemberForUpdate(memberId);

            if(member==null) return BadRequest("Could not get member");

            member.DisplayName=memberUpdatedDto.DisplayName ?? member.DisplayName;
            member.Description=memberUpdatedDto.Description ?? member.Description;
            member.City=memberUpdatedDto.City ?? member.City;
            member.Country=memberUpdatedDto.Country ?? member.Country;

            member.User.DisplayName=memberUpdatedDto.DisplayName ?? member.User.DisplayName;

            // memberRepository.Update(member);

            if(await memberRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update member");
        }
    }
}
