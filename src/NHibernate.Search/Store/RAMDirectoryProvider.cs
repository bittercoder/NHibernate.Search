using System;
using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;
using NHibernate.Search.Engine;
using Directory=Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;

namespace NHibernate.Search.Store
{
    public class RAMDirectoryProvider : IDirectoryProvider
    {
        private RAMDirectory directory;
        private string indexName;

        public Directory Directory
        {
            get { return directory; }
        }

        public void Initialize(String directoryProviderName, IDictionary<string, string> properties, ISearchFactoryImplementor searchFactory)
        {
            if (directoryProviderName == null)
                throw new ArgumentNullException("directoryProviderName");

            indexName = directoryProviderName;
            directory = new RAMDirectory();
            try
            {
                IndexWriter iw = new IndexWriter(directory, new StandardAnalyzer(LuceneVersion.Default), true, IndexWriter.MaxFieldLength.UNLIMITED);
                iw.Close();
                //searchFactory.RegisterDirectoryProviderForLocks(this);
            }
            catch (IOException e)
            {
                throw new HibernateException("Unable to initialize index: " + indexName, e);
            }
        }

        public void Start()
        {
            
        }

        public override bool Equals(Object obj)
        {
            // this code is actually broken since the value change after initialize call
            // but from a practical POV this is fine since we only call this method
            // after initialize call
            if (obj == this) return true;
            if (obj == null || !(obj is RAMDirectoryProvider)) return false;
            return indexName.Equals(((RAMDirectoryProvider) obj).indexName);
        }

        public override int GetHashCode()
        {
            // this code is actually broken since the value change after initialize call
            // but from a practical POV this is fine since we only call this method
            // after initialize call
            int hash = 7;
            return 29*hash + indexName.GetHashCode();
        }
    }
}