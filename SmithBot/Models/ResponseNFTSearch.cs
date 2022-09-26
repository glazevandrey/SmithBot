using System.Collections.Generic;

namespace SmithBot.Models
{
    public class AlphaNftItemSearch
    {
        public List<Edge> edges { get; set; }
        public Info info { get; set; }
        public string __typename { get; set; }
    }

    public class Attribute
    {
        public string traitType { get; set; }
        public string value { get; set; }
        public string __typename { get; set; }
    }

    public class Collection
    {
        public string name { get; set; }
        public string address { get; set; }
        public bool isRarityValid { get; set; }
        public bool? isVerified { get; set; }
        public bool hasRarityAttributes { get; set; }
        public bool isRarityEnabled { get; set; }
        public string __typename { get; set; }
    }

    public class Data
    {
        public AlphaNftItemSearch alphaNftItemSearch { get; set; }
    }

    public class Edge
    {
        public Node node { get; set; }
        public string cursor { get; set; }
        public string __typename { get; set; }
    }

    public class Image
    {
        public string baseUrl { get; set; }
        public string sized { get; set; }
        public bool hasAnimation { get; set; }
        public object animation { get; set; }
        public object preview { get; set; }
        public string __typename { get; set; }
    }

    public class Info
    {
        public bool hasNextPage { get; set; }
        public string __typename { get; set; }
    }

    public class Node
    {
        public string name { get; set; }
        public PreviewImage previewImage { get; set; }
        public string address { get; set; }
        public Owner owner { get; set; }
        public string ownerAddress { get; set; }
        public Collection collection { get; set; }
        public Sale sale { get; set; }
        public List<Attribute> attributes { get; set; }
        public ReactionCounters reactionCounters { get; set; }
        public int rarityRank { get; set; }
        public string __typename { get; set; }
    }

    public class Owner
    {
        public string wallet { get; set; }
        public string __typename { get; set; }
    }

    public class PreviewImage
    {
        public Image image { get; set; }
        public string __typename { get; set; }
    }

    public class ReactionCounters
    {
        public int likes { get; set; }
        public string __typename { get; set; }
    }

    public class ResponseNFTSearch
    {
        public Data data { get; set; }
    }

    public class Sale
    {
        public string minBid { get; set; }
        public string maxBid { get; set; }
        public object lastBidAmount { get; set; }
        public int finishAt { get; set; }
        public bool end { get; set; }
        public string __typename { get; set; }
        public string fullPrice { get; set; }
        public string nftOwnerAddress { get; set; }
    }
}
