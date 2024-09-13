using TodoApp.Api.Entities;
using TodoApp.Api.Dtos.Tag;

namespace TodoApp.Api.Mapping;

public static class TagMapping
{
    public static Tag ToEntity(this CreateTagDto tagDto)
    {
        return new Tag
        {
            Name = tagDto.Name
        };
    }

    public static TagDto ToTagDto(this Tag tag)
    {
        return new TagDto(
            tag.Id,
            tag.Name
        );
    }

    public static void UpdateEntityFromDto(this Tag tag, CreateTagDto updateDto)
    {
        tag.Name = updateDto.Name;
        // Note: Be cautious about updating CreatedAt or LastLoginAt here
    }
}