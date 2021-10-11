using System;
using System.Collections.Generic;
using CrossFitLibrary.Api.Controllers;
using CrossFitLibrary.Api.Models;

namespace CrossFitLibrary.Api
{
    public class TrickyStore
    {
        private List<Trick> _tricks;

        public TrickyStore()
        {
            _tricks = new List<Trick>();
        }

        public IEnumerable<Trick> All => _tricks;

        public void Add(Trick trick)
        {
            trick.Id = _tricks.Count + 1;
            _tricks.Add(trick);
        }
    }
}