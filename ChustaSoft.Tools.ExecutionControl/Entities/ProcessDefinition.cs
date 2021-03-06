﻿using ChustaSoft.Common.Helpers;
using ChustaSoft.Tools.ExecutionControl.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.ExecutionControl.Entities
{
    public class ProcessDefinition<TKey> : IEquatable<ProcessDefinition<TKey>> where TKey : IComparable
    {

        public TKey Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ProcessType Type { get; set; }
        public bool Active { get; set; }


        public ICollection<Execution<TKey>> Executions { get; set; }


        public ProcessDefinition()
        {
            Executions = Enumerable.Empty<Execution<TKey>>().ToList();
        }


        public bool Equals(ProcessDefinition<TKey> other) => other.Name.Equals(this.Name);

        public TProcessEnum GetEnumDefinition<TProcessEnum>()
            where TProcessEnum : struct, IConvertible
        {
            return EnumsHelper.GetByString<TProcessEnum>(this.Name);
        }

    }
}
