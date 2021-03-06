﻿using System;
using System.Collections.Generic;
using System.Linq;
using JoinRpg.DataModel;

namespace JoinRpg.Domain
{
  public static class FieldExtensions
  {
    public static bool HasValueList(this ProjectField field)
    {
      return field.FieldType == ProjectFieldType.Dropdown || field.FieldType == ProjectFieldType.MultiSelect;
    }

    public static bool HasSpecialGroup(this ProjectField field)
    {
      return field.HasValueList() && field.FieldBoundTo == FieldBoundTo.Character;
    }

    public static IReadOnlyList<ProjectFieldDropdownValue> GetDropdownValues(this FieldWithValue field)
    {
      var value = field.GetSelectedIds();
      return field.GetPossibleValues().Where(
        v => value.Contains(v.ProjectFieldDropdownValueId)).ToList().AsReadOnly();
    }

    private static IEnumerable<int> GetSelectedIds(this FieldWithValue field)
    {
      return String.IsNullOrWhiteSpace(field.Value) ? Enumerable.Empty<int>() : field.Value.Split(',').Select(Int32.Parse);
    }

    public static IEnumerable<ProjectFieldDropdownValue> GetPossibleValues(this FieldWithValue field)
    {
      var value = field.GetSelectedIds();
      return field.Field.GetOrderedValues().Where(v => v.IsActive || value.Contains(v.ProjectFieldDropdownValueId));
    }

    public static string GetSpecialGroupName(this ProjectFieldDropdownValue fieldValue)
    {
      return $"{fieldValue.Label}";
    }

    public static string GetSpecialGroupName(this ProjectField field)
    {
      return $"{field.FieldName}";
    }

    public static bool HasValue(this FieldWithValue ch)
    {
      return !String.IsNullOrWhiteSpace(ch.Value) || ch.Field.FieldType == ProjectFieldType.Header;
    }
  }
}
