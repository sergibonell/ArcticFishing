using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<RopeLineSegment> ropeSegments = new List<RopeLineSegment>();

    [SerializeField] private float segLength = .25f;
    private int segNumber = 35;
    [SerializeField] private float lineWidth = .1f;
    [SerializeField] private Vector3 gravity = new Vector3(0, -1, 0);
    [SerializeField] private int simulationLoops = 50;
    [SerializeField] private Transform followTarget;
    [SerializeField] private Transform anchorTarget;
    [SerializeField] public float addSegmentThreshold;
    [SerializeField] private int minNumberSegments;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = followTarget.position;

        for(int i = 0; i < segNumber; i++)
        {
            ropeSegments.Add(new RopeLineSegment(ropeStartPoint));
            ropeStartPoint.y -= segLength;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Simulate();
        DrawRope();
    }

    private void DrawRope()
    {
        // 0 = target
        // n = anchor
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePoints = new Vector3[segNumber];
        for(int i = 0; i < segNumber; i++)
        {
            ropePoints[i] = ropeSegments[i].curPos;
        }

        lineRenderer.positionCount = ropePoints.Length;
        lineRenderer.SetPositions(ropePoints);
    }

    private void Simulate()
    {
        // SIMULATION
        for(int i = 0; i < segNumber; i++)
        {
            RopeLineSegment segment = ropeSegments[i];
            Vector3 velocity = segment.curPos - segment.lastPos;

            segment.lastPos = segment.curPos;
            segment.curPos += velocity;
            segment.curPos += gravity * Time.deltaTime;
            ropeSegments[i] = segment;
        }

        // CONSTRAINT
        for(int i = 0; i < simulationLoops; i++)
        {
            Constraint();
        }
    }

    private void Constraint()
    {
        // VARIABLES
        RopeLineSegment firstSeg;
        RopeLineSegment secondSeg;
        Vector3 change;

        // FIRST POINT
        firstSeg = ropeSegments[0];
        secondSeg = ropeSegments[1];
        change = calculateChangeAmount(firstSeg, secondSeg);

        firstSeg.curPos = followTarget.position;
        ropeSegments[0] = firstSeg;
        secondSeg.curPos += change;
        ropeSegments[1] = secondSeg;

        // LAST POINT
        firstSeg = ropeSegments[segNumber - 2];
        secondSeg = ropeSegments[segNumber - 1];
        change = calculateChangeAmount(firstSeg, secondSeg);

        firstSeg.curPos -= change;
        ropeSegments[segNumber - 2] = firstSeg;
        secondSeg.curPos = anchorTarget.position;
        ropeSegments[segNumber - 1] = secondSeg;

        // INTERMEDIATE POINTS
        for (int i = 1; i < segNumber - 2; i++)
        {
            firstSeg = ropeSegments[i];
            secondSeg = ropeSegments[i + 1];
            change = calculateChangeAmount(firstSeg, secondSeg);
            
            firstSeg.curPos -= change * 0.5f;
            ropeSegments[i] = firstSeg;
            secondSeg.curPos += change * 0.5f;
            ropeSegments[i + 1] = secondSeg;
        }

        // ADD OR REMOVE SEGMENTS DEPENDING ON ROPE LENGTH
        float ropeLength = segNumber * segLength;
        float targetDistance = (followTarget.position - anchorTarget.position).magnitude;

        if (ropeLength < targetDistance + addSegmentThreshold)
        {
            // LENGTHEN ROPE
            RopeLineSegment last = ropeSegments[segNumber - 1];
            RopeLineSegment secondToLast = ropeSegments[segNumber - 2];
            Vector3 ropeDirection = (last.curPos - secondToLast.curPos).normalized;
            Vector3 newPoint = ropeDirection * segLength;
            // New items are added at end
            ropeSegments.Add(new RopeLineSegment(last.curPos + newPoint));
            segNumber++;
        }else if(ropeLength - addSegmentThreshold - 0.5f > targetDistance && segNumber > minNumberSegments)
        {
            // SHORTEN ROPE
            ropeSegments.RemoveAt(segNumber - 1);
            segNumber--;
        }
    }

    private Vector3 calculateChangeAmount(RopeLineSegment first, RopeLineSegment second)
    {
        float dist = (first.curPos - second.curPos).magnitude;
        float error = dist - segLength;
        Vector3 changeDir = (first.curPos - second.curPos).normalized;
        Vector3 changeAmount = changeDir * error;
        return changeAmount;
    }

    public struct RopeLineSegment
    {
        public Vector3 curPos;
        public Vector3 lastPos;

        public RopeLineSegment(Vector3 pos)
        {
            curPos = pos;
            lastPos = pos;
        }
    }
}
